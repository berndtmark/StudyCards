import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { LoadingState } from 'app/shared/models/loading-state';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { CardService } from '../services/card.service';
import { SnackbarService } from 'app/shared/services/snackbar.service';
import { CardResponse } from 'app/@api/models/card-response';
import { ImportCard } from '../models/import-card';
import { DialogService } from 'app/shared/services/dialog.service';
import { FileService } from 'app/shared/services/file.service';
import { CardText } from 'app/@api/models/card-text';

type CardState = {
    loadingState: LoadingState
    cards: CardResponse[];
    deckId: string;
    importCards: { file?: ImportCard[], success?: ImportCard[], existing?: ImportCard[] };
};

const initialState: CardState = {
    loadingState: LoadingState.Initial,
    cards: [],
    deckId: '',
    importCards: {}
};

export const CardStore = signalStore(
    withState(initialState),
    withComputed((store) => ({
        cardCount: computed(() => store.cards().length),
        isImportStared: computed(() => store.importCards().file),
        isLoading: computed(() => store.loadingState() === LoadingState.Loading)
    })),
    withMethods((store,
        cardService = inject(CardService),
        snackBar = inject(SnackbarService),
        dialogService = inject(DialogService),
        fileService = inject(FileService)) => ({
            loadCards: rxMethod<string>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((deckId) => {
                        return cardService.getCards(deckId).pipe(
                            tap((cards) => {
                                patchState(store, {
                                    cards,
                                    loadingState: LoadingState.Success,
                                    deckId
                                });
                            }),
                            catchError(() => {
                                patchState(store, { loadingState: LoadingState.Error });
                                return of([]);
                            })
                        );
                    })
                )
            ),
            addCard: rxMethod<{ cardFront: string, cardBack: string }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((card) => cardService.addCard(store.deckId(), card.cardFront, card.cardBack).pipe(
                        tap((newCard) => {
                            patchState(store, (state) => ({ 
                                cards: [...state.cards, newCard],
                                loadingState: LoadingState.Success 
                            }));
                            snackBar.open("Card added successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to add card");
                            return of(null);
                        })
                    ))
                )
            ),
            updateCard: rxMethod<{ cardId: string, cardFront: string, cardBack: string }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((card) => cardService.updateCard(store.deckId(), card.cardId, card.cardFront, card.cardBack).pipe(
                        tap((updatedCard) => {
                            patchState(store, (state) => {
                                const updatedCards = state.cards.map((card) =>
                                    card.id === updatedCard.id ? updatedCard : card
                                );
                                return {
                                    ...state,
                                    cards: updatedCards,
                                    loadingState: LoadingState.Success
                                };
                            });
                            snackBar.open("Card updated successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to update card");
                            return of(null);
                        })
                    ))
                )
            ),
            removeCard: rxMethod<string>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((cardId) => cardService.removeCard(store.deckId(), cardId).pipe(
                        tap(() => {
                            patchState(store, (state) => {
                                const updatedCards = state.cards.filter(card => card.id !== cardId);
                                return {
                                    ...state,
                                    cards: updatedCards,
                                    loadingState: LoadingState.Success
                                };
                            });
                            snackBar.open("Card removed successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to remove card");
                            return of(null);
                        })
                    ))
                )
            ),
            getCardById: (id: string): CardResponse | null => {
                return store.cards().find(card => card.id === id) || null;
            },
            importCardsFromFile: async (event: Event) => {
                patchState(store, { importCards: {} });

                try {
                    const data = await fileService.readJsonFile<ImportCard[]>(event);
                    const isValid = ImportCard.isValid(data);
                    if (!isValid)
                        throw new Error();

                    patchState(store, { 
                        importCards: { 
                            file: data.map(d => ({...d, id: crypto.randomUUID()})),
                            success: [],
                            existing: []
                        } 
                    });
                } catch (error) {
                    dialogService.info("Error Importing", "The File is not in a recognisable format or is badly formed");
                }
            },
            import: rxMethod<{ cardsIdsToImport: string[] }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(({cardsIdsToImport}) => { 
                        const cardsToImport = store.importCards().file?.filter(f => f.id && cardsIdsToImport.includes(f.id)) ?? [];

                        return cardService.addCards(store.deckId(), cardsToImport)
                            .pipe(
                                tap((importedCards) => {
                                    patchState(store, { 
                                        loadingState: LoadingState.Success,
                                        importCards: { 
                                            file: [],
                                            success: [...importedCards.cardsAdded!],
                                            existing: [...importedCards.cardsSkipped!]
                                        },
                                        cards: [...store.cards(), ...importedCards.cardsAdded!]
                                    });
                                    snackBar.open("Cards import ran successfully");
                                }),
                                catchError(() => {
                                    patchState(store, { loadingState: LoadingState.Error });
                                    snackBar.open("Failed to import cards");
                                    return of(null);
                                })
                        )}
                    )
                )
            ),
            export: () => {
                const data = store.cards().map(c => ({ cardFront: c.cardFront, cardBack: c.cardBack }));
                fileService.downloadJson(data, 'studycards.json');
            }
    })),
    withMethods((store) => ({
        loadDeckIfNot: (deckId: string) => {
            if (store.deckId() !== deckId)
                store.loadCards(deckId);
        }
    })),
);