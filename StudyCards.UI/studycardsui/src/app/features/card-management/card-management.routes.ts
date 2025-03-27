import { Routes } from "@angular/router";
import { CardStore } from "./store/card.store";
import { CardListComponent } from "./components/card-list/card-list.component";

export const CARD_MANAGEMENT_ROUTES: Routes = [
    {
        path: '',
        providers: [CardStore],
        children: [
            {
                path: '',
                component: CardListComponent
            }
        ]
    }
]