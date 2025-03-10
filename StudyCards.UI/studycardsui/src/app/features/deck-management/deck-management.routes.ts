import { Routes } from "@angular/router";
import { DeckListComponent } from "./components/deck-list/deck-list.component";

export const DECK_MANAGEMENT_ROUTES: Routes = [
    {
        path: '',
        component: DeckListComponent
    }
]