import { signalStore, withState } from "@ngrx/signals";
import { CardResponse } from "app/@api/models/card-response";
import { LoadingState } from "app/shared/models/loading-state";

type StudyState = {
    loadingState: LoadingState
    cards: CardResponse[];
};

const initialState: StudyState = {
    loadingState: LoadingState.Initial,
    cards: [],
};

export const StudyStore = signalStore(
    withState(initialState),
);