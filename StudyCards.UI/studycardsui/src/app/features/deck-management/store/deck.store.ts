import { patchState, signalStore, withComputed, withMethods, withState, withHooks } from '@ngrx/signals';
import { Deck } from '../models/deck.model';
import { computed, inject } from '@angular/core';
import { DeckService } from '../services/deck.service';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { LoadingState } from 'app/shared/models/loading-state';
import { SnackbarService } from 'app/shared/services/snackbar.service';

type DeckState = {
    loadingState: LoadingState
    decks: Deck[];
};

const initialState: DeckState = {
    loadingState: LoadingState.Initial,
    decks: []
};

export const DeckStore = signalStore(
    withState(initialState),
    withComputed(({ decks }) => ({
        deckCount: computed(() => decks().length)
    })),
    withMethods((store, 
        snackBar = inject(SnackbarService),
        deckService = inject(DeckService)) => ({
            loadDecks: rxMethod<void>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(() => deckService.getDecks().pipe(
                        tap((decks) => {
                            patchState(store, { 
                                decks,
                                loadingState: LoadingState.Success 
                            });
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            return of([]);
                        })
                    ))
                )
            ),
            addDeck(deck: Deck): void {
                snackBar.open("Saved");
                patchState(store, (state) => ({ 
                    decks: [...state.decks, deck]
                }));
            }
        }),
    ),
    withHooks({
        onInit(store) {
            store.loadDecks();
        }
    })
);