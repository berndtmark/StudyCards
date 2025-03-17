import { Routes } from "@angular/router";
import { DeckListComponent } from "./components/deck-list/deck-list.component";
import { DeckStore } from "./store/deck.store";
import { AddDeckFormComponent } from "./components/add-update-deck-form/add-deck-form/add-deck-form.component";
import { UpdateDeckFormComponent } from "./components/add-update-deck-form/update-deck-form/update-deck-form.component";

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
                component: AddDeckFormComponent
            },
            {
                path: 'edit/:deckid',
                component: UpdateDeckFormComponent
            }
        ]
    }
]