import { patchState, signalStore, withComputed, withMethods, withState } from '@ngrx/signals';
import { LoadingState } from 'app/shared/models/loading-state';
import { Card } from 'app/@api/models/card';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { CardService } from '../services/card.service';
import { SnackbarService } from 'app/shared/services/snackbar.service';

type CardState = {
    loadingState: LoadingState
    cards: Card[];
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
            cardCountByDeckId: (deckId: string) =>  
                store.cards().filter(card => card.deckId === deckId).length,
    })),
);