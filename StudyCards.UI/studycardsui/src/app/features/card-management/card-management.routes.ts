import { Routes } from "@angular/router";
import { CardStore } from "./store/card.store";
import { CardLandingComponent } from "./components/card-landing/card-landing.component";
import { AddCardComponent } from "./components/add-card/add-card.component";
import { UpdateCardComponent } from "./components/update-card/update-card.component";

export const CARD_MANAGEMENT_ROUTES: Routes = [
    {
        path: '',
        providers: [CardStore],
        children: [
            {
                path: '',
                component: CardLandingComponent
            },
            {
                path: 'add',
                component: AddCardComponent
            },
            {
                path: 'edit/:cardid',
                component: UpdateCardComponent
            }
        ]
    }
]