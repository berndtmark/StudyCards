import { Routes } from "@angular/router";
import { CardStore } from "./store/card.store";
import { CardLandingComponent } from "./components/card-landing/card-landing.component";
import { AddCardFormComponent } from "./components/add-update-card-form/add-card-form/add-card-form.component";
import { UpdateCardFormComponent } from "./components/add-update-card-form/update-card-form/update-card-form.component";

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
                component: AddCardFormComponent
            },
            {
                path: 'edit/:cardid',
                component: UpdateCardFormComponent
            }
        ]
    }
]