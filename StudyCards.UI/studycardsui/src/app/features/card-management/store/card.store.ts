import { signalStore, withState } from '@ngrx/signals';
import { LoadingState } from 'app/shared/models/loading-state';
import { Card } from 'app/@api/models/card';

type CardState = {
    loadingState: LoadingState
    cards: Card[];
};

const initialState: CardState = {
    loadingState: LoadingState.Initial,
    cards: []
};

export const CardStore = signalStore(
    withState(initialState)
);