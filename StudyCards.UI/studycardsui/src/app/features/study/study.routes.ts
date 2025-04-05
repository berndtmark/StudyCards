import { Routes } from "@angular/router";
import { StudyStore } from "./store/study.store";
import { StudyLandingComponent } from "./components/study-landing/study-landing.component";
import { StudySessionComponent } from "./components/study-session/study-session.component";

export const STUDY_ROUTES: Routes = [
    {
        path: '',
        providers: [StudyStore],
        children: [
            {
                path: '',
                component: StudyLandingComponent
            },
            {
                path: 'session/:methodology',
                component: StudySessionComponent
            }
        ]
    }
]