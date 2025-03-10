import { patchState, signalStore, withComputed, withMethods, withState, withHooks } from '@ngrx/signals';
import { Deck } from '../models/deck.model';
import { computed, inject } from '@angular/core';
import { DeckService } from '../services/deck.service';
import { pipe, switchMap, tap } from 'rxjs';
import { rxMethod } from '@ngrx/signals/rxjs-interop';

type DeckState = {
    decks: Deck[];
};

const initialState: DeckState = {
    decks: []
};

export const DeckStore = signalStore(
    withState(initialState),
    withComputed(({ decks }) => ({
        deckCount: computed(() => decks().length)
    })),
    withMethods((store, 
        deckService = inject(DeckService)) => ({
            loadDecks: rxMethod<void>(
                pipe(
                    switchMap(() => deckService.getDecks()),
                    tap((decks) => patchState(store, { decks }))
            )),
            addDeck(deck: Deck): void {
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