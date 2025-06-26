import { Routes } from "@angular/router";
import { DeckListComponent } from "./components/deck-list/deck-list.component";
import { DeckStore } from "./store/deck.store";
import { AddDeckComponent } from "./components/add-deck/add-deck.component";
import { UpdateDeckComponent } from "./components/update-deck/update-deck.component";

export const DECK_MANAGEMENT_ROUTES: Routes = [
    {
        path: '',
        providers: [DeckStore],
        children: [
            {
                path: '',
                component: DeckListComponent
            },
            {
                path: 'add',
                component: AddDeckComponent
            },
            {
                path: 'edit/:deckid',
                component: UpdateDeckComponent
            }
        ]
    }
]