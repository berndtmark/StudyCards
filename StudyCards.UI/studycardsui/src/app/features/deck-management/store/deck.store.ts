import { patchState, signalStore, withComputed, withMethods, withState, withHooks } from '@ngrx/signals';
import { computed, effect, inject } from '@angular/core';
import { DeckService } from '../services/deck.service';
import { catchError, of, pipe, switchMap, tap } from 'rxjs';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { LoadingState } from 'app/shared/models/loading-state';
import { SnackbarService } from 'app/shared/services/snackbar.service';
import { DeckResponse } from 'app/@api/models/deck-response';
import { DeckEventsService } from '../services/deck-events.service';

type DeckState = {
    loadingState: LoadingState
    decks: DeckResponse[];
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
            addDeck: rxMethod<DeckResponse>(
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
            updateDeck: rxMethod<DeckResponse>(
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
            removeDeck: rxMethod<string>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((deckId) => deckService.removeDeck(deckId).pipe(
                        tap(() => {
                            patchState(store, (state) => {
                                const updatedDecks = state.decks.filter(deck => deck.id !== deckId);
                                return {
                                    ...state,
                                    decks: updatedDecks,
                                    loadingState: LoadingState.Success
                                };
                            });
                            snackBar.open("Deck removed successfully");
                        }),
                        catchError(() => {
                            patchState(store, { loadingState: LoadingState.Error });
                            snackBar.open("Failed to remove deck");
                            return of(null);
                        })
                    ))
                )
            ),
            getDeckById: (id: string) => {
                return store.decks().find(deck => deck.id === id) || null;
            },
            deckReviewed: (deckId: string): void => {
                patchState(store, (state) => ({
                    decks: state.decks.map((deck) =>
                        deck.id === deckId ? 
                        { ...deck, 
                            deckReviewStatus: {
                                ...deck.deckReviewStatus,
                                lastReview: new Date().toISOString()
                            }
                        } :
                        deck)
                }));
            }
        }),
    ),
    withHooks({
        onInit(store, 
            deckEvents = inject(DeckEventsService)) {
                store.loadDecks();

                effect(() => {
                    const deckId = deckEvents.reviewCompleted();
                    if (deckId) {
                        store.deckReviewed(deckId);
                    }
                });
        }
    })
);