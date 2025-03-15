import { Routes } from "@angular/router";
import { DeckListComponent } from "./components/deck-list/deck-list.component";
import { AddDeckFormComponent } from "./components/add-deck-form/add-deck-form.component";
import { DeckStore } from "./store/deck.store";

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
            }
        ]
    }
]