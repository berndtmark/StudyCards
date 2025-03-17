import { patchState, signalStore, withComputed, withMethods, withState, withHooks } from '@ngrx/signals';
import { computed, inject } from '@angular/core';
import { DeckService } from '../services/deck.service';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { LoadingState } from 'app/shared/models/loading-state';
import { SnackbarService } from 'app/shared/services/snackbar.service';
import { Deck } from 'app/@api/models/deck';

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
            addDeck: rxMethod<Deck>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((deck) => deckService.addDeck(deck).pipe(
                        tap((newDeck) => {
                            patchState(store, (state) => ({ 
                                decks: [...state.decks, newDeck],
                                loadingState: LoadingState.Success 
                            }));
                            snackBar.open("Deck added successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to add deck");
                            return of(null);
                        })
                    ))
                )
            ),
            updateDeck: rxMethod<Deck>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((deck) => deckService.updateDeck(deck).pipe(
                        tap((updatedDeck) => {
                            patchState(store, (state) => {
                                const updatedDecks = state.decks.map((deck) =>
                                    deck.id === updatedDeck.id ? updatedDeck : deck
                                );
                                return {
                                    ...state,
                                    decks: updatedDecks,
                                    loadingState: LoadingState.Success
                                };
                            });
                            snackBar.open("Deck updated successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to update deck");
                            return of(null);
                        })
                    ))
                )
            ),
            getDeckById: (id: string) => {
                return store.decks().find(deck => deck.id === id) || null;
            }
        }),
    ),
    withHooks({
        onInit(store) {
            store.loadDecks();
        }
    })
);