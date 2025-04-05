import { inject } from "@angular/core";
import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { CardResponse } from "app/@api/models/card-response";
import { LoadingState } from "app/shared/models/loading-state";
import { StudyService } from "../services/study.service";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { catchError, of, pipe, switchMap, tap } from "rxjs";
import { StudyMethodology } from "app/shared/models/study-methodology";

type StudyState = {
    loadingState: LoadingState
    cards: CardResponse[];
    deckId: string;
};

const initialState: StudyState = {
    loadingState: LoadingState.Initial,
    cards: [],
    deckId: ''
};

export const StudyStore = signalStore(
    withState(initialState),
    withMethods((store,
        studyService = inject(StudyService)) => ({
            init: (deckId: string) => patchState(store, { deckId }),
            loadCards: rxMethod<{ methodology: StudyMethodology }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((study) => {
                        return studyService.getStudyCards(store.deckId(), study.methodology).pipe(
                            tap((cards) => {
                                patchState(store, {
                                    cards,
                                    loadingState: LoadingState.Success,
                                });
                            }),
                            catchError(() => {
                                patchState(store, { loadingState: LoadingState.Error });
                                return of([]);
                            })
                        );
                    })
                )
            ),
        })
    )
);