import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { LoadingState } from 'app/shared/models/loading-state';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { CardService } from '../services/card.service';
import { SnackbarService } from 'app/shared/services/snackbar.service';
import { CardResponse } from 'app/@api/models/card-response';

type CardState = {
    loadingState: LoadingState
    cards: CardResponse[];
    deckId: string;
};

const initialState: CardState = {
    loadingState: LoadingState.Initial,
    cards: [],
    deckId: ''
};

export const CardStore = signalStore(
    withState(initialState),
    withComputed(({ cards }) => ({
        cardCount: computed(() => cards().length)
    })),
    withMethods((store,
        cardService = inject(CardService),
        snackBar = inject(SnackbarService)) => ({
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
            cardCountByDeckId: (deckId: string) =>  
                store.cards().filter(card => card.deckId === deckId).length,
            getCardById: (id: string) => {
                return store.cards().find(card => card.id === id) || null;
            }
    })),
);