import { patchState, signalStore, withMethods, withState } from "@ngrx/signals";
import { LoadingState } from "../../../shared/models/loading-state";
import { StatisticService } from "../services/statistic.service";
import { inject } from "@angular/core";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { filter, pipe, switchMap, tap } from "rxjs";
import { ErrorHandlerService } from "../../../shared/services/error-handler.service";
import { tapResponse } from '@ngrx/operators';
import { StudyStatisticResponse } from "../../../@api/models/study-statistic-response";
import { DateFuctions } from "../../../shared/functions/date-functions";

type StatisticState = {
    loadingState: LoadingState;
    studyStatistics: StudyStatisticResponse[],
    monthYearLoaded: string[]
}

const initialState: StatisticState = {
    loadingState: LoadingState.Initial,
    studyStatistics: [],
    monthYearLoaded: []
};

export const StatisticStore = signalStore(
    withState(initialState),
    withMethods((store,
        statisticService = inject(StatisticService),
        errorHandler = inject(ErrorHandlerService)) => ({
            loadStudyStatisticsForMonth: rxMethod<{ date: Date }>(
                pipe(
                    filter(s => {
                        const loadedKey = DateFuctions.uniqueMonthKey(s.date);
                        return !store.monthYearLoaded().includes(loadedKey);
                    }),
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(s => { 
                        const startOfMonth = DateFuctions.firstDayOfMonth(s.date);
                        const endOfMonth = DateFuctions.lastDayOfMonth(s.date);
                        const monthYearKey = DateFuctions.uniqueMonthKey(s.date);

                        return statisticService.getStudyStatistics(startOfMonth, endOfMonth).pipe(
                            tapResponse({
                                next: (stats) => {        
                                    patchState(store, (state) => ({ 
                                        loadingState: LoadingState.Success, 
                                        studyStatistics: [...state.studyStatistics, ...stats],
                                        monthYearLoaded: [...state.monthYearLoaded, monthYearKey]
                                    }));
                                },
                                error: (error) => {
                                    errorHandler.handleTapError(store, "Failed to load study statistics", error);
                                }
                            })
                        )
                    })
                )
            ),
    }))
);