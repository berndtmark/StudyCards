import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { LoadingState } from "../../../shared/models/loading-state";
import { StatisticService } from "../services/statistic.service";
import { inject } from "@angular/core";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { pipe, switchMap, tap } from "rxjs";
import { ErrorHandlerService } from "../../../shared/services/error-handler.service";
import { tapResponse } from '@ngrx/operators';
import { StudyStatisticResponse } from "../../../@api/models/study-statistic-response";

type StatisticState = {
    loadingState: LoadingState;
    studyStatistics: StudyStatisticResponse[]
}

const initialState: StatisticState = {
    loadingState: LoadingState.Initial,
    studyStatistics: []
};

export const StatisticStore = signalStore(
    withState(initialState),
    withMethods((store,
        statisticService = inject(StatisticService),
        errorHandler = inject(ErrorHandlerService)) => ({
            loadStudyStatistics: rxMethod<{ from: Date, to: Date }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(s => statisticService.getStudyStatistics(s.from, s.to).pipe(
                        tapResponse({
                            next: (stats) => {        
                                patchState(store, { 
                                    loadingState: LoadingState.Success, 
                                    studyStatistics: stats 
                                });
                            },
                            error: (error) => {
                                errorHandler.handleTapError(store, "Failed to load study statistics", error);
                            }
                        })
                    ))
                )
            ),
    }))
);