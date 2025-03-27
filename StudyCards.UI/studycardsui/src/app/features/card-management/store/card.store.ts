import { patchState, signalStore, withComputed, withHooks, withMethods, withState } from '@ngrx/signals';
import { LoadingState } from 'app/shared/models/loading-state';
import { Card } from 'app/@api/models/card';
import { computed, inject } from '@angular/core';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { CardService } from '../services/card.service';

type CardState = {
    loadingState: LoadingState
    cards: Card[];
};

const initialState: CardState = {
    loadingState: LoadingState.Initial,
    cards: []
};

export const CardStore = signalStore(
    withState(initialState),
    withComputed(({ cards }) => ({
        cardCount: computed(() => cards().length)
    })),
    withMethods((store,
        cardService = inject(CardService)) => ({
            loadCards: rxMethod<string>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((deckId) => {
                        return cardService.getCards(deckId).pipe(
                            tap((cards) => {
                                patchState(store, {
                                    cards,
                                    loadingState: LoadingState.Success
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
    })),
);