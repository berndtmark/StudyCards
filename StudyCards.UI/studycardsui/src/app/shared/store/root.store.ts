import { patchState, signalStore, withComputed, withMethods, withState, withHooks } from '@ngrx/signals';
import { inject } from '@angular/core';
import { pipe, switchMap, tap } from 'rxjs';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { AuthorizeService } from '../services/authorize.service';
import { Claims } from '../constants/claims';

type RootState = {
    userName: string;
};

const initialState: RootState = {
    userName: ''
};

export const RootStore = signalStore(
    withState(initialState),
    withMethods((store,
        authService = inject(AuthorizeService)) => ({
            loadUser: rxMethod<void>(
                pipe(
                    switchMap(() => authService.claims().pipe(
                        tap(claims => patchState(store, { userName: claims.get(Claims.Name) }))
                    ))
                )
            )
        }),
    ),
    withHooks({
        onInit(store) {
            store.loadUser();
        }
    })
);