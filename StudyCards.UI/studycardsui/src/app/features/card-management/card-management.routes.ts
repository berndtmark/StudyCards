import { Routes } from "@angular/router";
import { CardStore } from "./store/card.store";
import { CardLandingComponent } from "./card-landing/card-landing.component";

export const CARD_MANAGEMENT_ROUTES: Routes = [
    {
        path: '',
        providers: [CardStore],
        children: [
            {
                path: '',
                component: CardLandingComponent
            }
        ]
    }
]