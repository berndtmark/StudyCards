import { Routes } from "@angular/router";
import { StatisticsLandingComponent } from "./components/statistics-landing/statistics-landing.component";

export const STATISTICS_ROUTES: Routes = [
    {
        path: '',
        providers: [],
        children: [
            {
                path: '',
                component: StatisticsLandingComponent
            }
        ]
    }
]