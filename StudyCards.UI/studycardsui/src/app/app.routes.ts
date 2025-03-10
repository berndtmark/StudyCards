import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'decks',
        pathMatch: 'full'
    },
    {
        path: 'decks',
        loadChildren: () => import('./features/deck-management/deck-management.routes')
          .then(m => m.DECK_MANAGEMENT_ROUTES)
    }
];
