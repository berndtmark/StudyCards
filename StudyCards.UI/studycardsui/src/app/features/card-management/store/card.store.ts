import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, debounceTime, pipe, switchMap, tap } from 'rxjs';
import { CardService } from '../services/card.service';
import { ImportCard } from '../models/import-card';
import { Router } from '@angular/router';
import { LoadingState } from '../../../shared/models/loading-state';
import { CardResponse } from '../../../@api/models/card-response';
import { Pagination } from '../../../shared/models/pagination';
import { SnackbarService } from '../../../shared/services/snackbar.service';
import { DialogService } from '../../../shared/services/dialog.service';
import { FileService } from '../../../shared/services/file.service';
import { ErrorHandlerService } from '../../../shared/services/error-handler.service';

type CardState = {
    loadingState: LoadingState
    cards: CardResponse[];
    deckId: string;
    importCards: { file?: ImportCard[], success?: ImportCard[], existing?: ImportCard[] };
    pagination: Pagination;
    searchTerm: string;
};

const initialState: CardState = {
    loadingState: LoadingState.Initial,
    cards: [],
    deckId: '',
    importCards: {},
    pagination: new Pagination(0, 1, 25),
    searchTerm: ''
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
        fileService = inject(FileService),
        errorHandler = inject(ErrorHandlerService),
        router = inject(Router)) => ({
            loadCards: rxMethod<{deckId: string, pageNumber?: number, pageSize?: number, searchTerm?: string}>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(({deckId, pageNumber, pageSize, searchTerm}) => {
                        return cardService.getCards(
                                deckId, 
                                pageNumber ?? store.pagination.pageNumber(), 
                                pageSize ?? store.pagination.pageSize(),
                                searchTerm)
                            .pipe(
                                tap((cards) => {
                                    patchState(store, {
                                        cards: cards.items,
                                        loadingState: LoadingState.Success,
                                        deckId,
                                        pagination: new Pagination(+cards.totalCount, +cards.pageNumber, +cards.pageSize),
                                        searchTerm
                                    });
                                }),
                                catchError(errorHandler.handleStoreError(store, "Failed to load cards"))
                            );
                    })
                )
            ),
            search: rxMethod<{ searchTerm: string }>(
                pipe(
                    debounceTime(300),
                    switchMap(({ searchTerm }) =>
                        cardService.getCards(store.deckId(), initialState.pagination.pageNumber, initialState.pagination.pageSize, searchTerm)
                            .pipe(
                                tap((cards) => {
                                    patchState(store, {
                                        cards: cards.items,
                                        loadingState: LoadingState.Success,
                                        pagination: new Pagination(+cards.totalCount, +cards.pageNumber, +cards.pageSize),
                                        searchTerm
                                    });
                                }),
                                catchError(errorHandler.handleStoreError(store, "Failed to find card"))
                            )
                    )
                )
            ),
            addCard: rxMethod<{ cardFront: string, cardBack: string }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((card) => cardService.addCard(store.deckId(), card.cardFront, card.cardBack).pipe(
                        tap((newCard) => {
                            patchState(store, {
                                cards: [...store.cards(), newCard],
                                loadingState: LoadingState.Success,
                                pagination: {
                                    ...store.pagination(),
                                    totalCount: store.pagination().totalCount + 1
                                }
                            });
                            snackBar.open("Card added successfully");
                            router.navigate(['/cards', store.deckId()]);
                        }),
                        catchError(errorHandler.handleStoreError(store, "Failed to add card"))
                    ))
                )
            ),
            updateCard: rxMethod<{ cardId: string, cardFront: string, cardBack: string }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((card) => cardService.updateCard(store.deckId(), card.cardId, card.cardFront, card.cardBack).pipe(
                        tap((updatedCard) => {
                            patchState(store, {
                                cards: store.cards().map(card =>
                                    card.id === updatedCard.id ? updatedCard : card
                                ),
                                loadingState: LoadingState.Success
                            });
                            snackBar.open("Card updated successfully");
                            router.navigate(['/cards', store.deckId()]);
                        }),
                        catchError(errorHandler.handleStoreError(store, "Failed to update card"))
                    ))
                )
            ),
            removeCard: rxMethod<string>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((cardId) => cardService.removeCard(store.deckId(), cardId).pipe(
                        tap(() => {
                            patchState(store, {
                                cards: store.cards().filter(card => card.id !== cardId),
                                loadingState: LoadingState.Success,
                                pagination: {
                                    ...store.pagination(),
                                    totalCount: store.pagination().totalCount - 1
                                }
                            });
                            snackBar.open("Card removed successfully");
                            router.navigate(['/cards', store.deckId()]);
                        }),
                        catchError(errorHandler.handleStoreError(store, "Failed to remove card"))
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
                                catchError(errorHandler.handleStoreError(store, "Failed to import cards"))
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
                store.loadCards({deckId});
        },
        paginate: (pageNumber: number, pageSize: number) => {
            if (!store.deckId())
                throw "Load Deck Before Paginating"

            store.loadCards({deckId: store.deckId(), pageNumber, pageSize});
        }
    })),
);