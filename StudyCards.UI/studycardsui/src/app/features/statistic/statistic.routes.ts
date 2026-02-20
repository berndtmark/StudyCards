import { Routes } from "@angular/router";
import { StatisticsLandingComponent } from "./components/statistics-landing/statistics-landing.component";
import { StatisticStore } from "./store/statistic.store";

export const STATISTIC_ROUTES: Routes = [
    {
        path: '',
        providers: [StatisticStore],
        children: [
            {
                path: '',
                component: StatisticsLandingComponent
            }
        ]
    }
]