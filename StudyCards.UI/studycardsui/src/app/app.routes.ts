import { Routes } from '@angular/router';
import { AuthGuard } from './shared/guards/auth-guard';
import { LoginComponent } from './shared/components/login/login.component';
import { ShellPageComponent } from './shared/components/shell-page/shell-page.component';
import { RootStore } from './shared/store/root.store';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'decks',
        pathMatch: 'full'
    },
    {
        path: '',
        providers: [RootStore],
        component: ShellPageComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: 'decks',
                loadChildren: () => import('./features/deck-management/deck-management.routes')
                    .then(m => m.DECK_MANAGEMENT_ROUTES)
            },
            {
                path: 'cards/:deckid',
                loadChildren: () => import('./features/card-management/card-management.routes')
                    .then(m => m.CARD_MANAGEMENT_ROUTES)
            }
        ]
    },
    {
        'path': 'login',
        component: LoginComponent
    }
];
