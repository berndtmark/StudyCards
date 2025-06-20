import { computed, inject } from "@angular/core";
import { patchState, signalStore, withComputed, withMethods, withState } from "@ngrx/signals";
import { CardResponse } from "app/@api/models/card-response";
import { LoadingState } from "app/shared/models/loading-state";
import { StudyService } from "../services/study.service";
import { rxMethod } from "@ngrx/signals/rxjs-interop";
import { catchError, of, pipe, switchMap, tap } from "rxjs";
import { StudyMethodology } from "app/shared/models/study-methodology";
import { CardDifficulty } from "app/shared/models/card-difficulty";
import { SnackbarService } from "app/shared/services/snackbar.service";
import { Router } from "@angular/router";
import { DeckEventsService } from "app/features/deck-management/services/deck-events.service";

type StudyState = {
    loadingState: LoadingState
    cards: CardResponse[];
    deckId: string;
    cardsStudied: { cardId: string; cardDifficulty: CardDifficulty }[];
};

const initialState: StudyState = {
    loadingState: LoadingState.Initial,
    cards: [],
    deckId: '',
    cardsStudied: [],
};

export const StudyStore = signalStore(
    withState(initialState),
    withComputed((store) => ({
        hasCardsToStudy: computed(() => store.cards().length > 0),
        isLoading: computed(() => store.loadingState() === LoadingState.Loading),
    })),
    withMethods((store,
        studyService = inject(StudyService),
        snackBarService = inject(SnackbarService),
        route = inject(Router),
        deckEvents = inject(DeckEventsService)) => ({
            loadCards: rxMethod<{ deckId: string, methodology: StudyMethodology }>(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap((study) => {
                        return studyService.getStudyCards(study.deckId, study.methodology).pipe(
                            tap((cards) => {
                                patchState(store, {
                                    cards,
                                    cardsStudied: [],
                                    loadingState: LoadingState.Success,
                                    deckId: study.deckId,
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
            repeatCard: (cardId: string) => {
                const card = store.cards().find(card => card.id === cardId);
                if (card) {
                    const repeatedCard = { ...card, cardDifficulty: CardDifficulty.Repeat };
                    patchState(store, { 
                        cards: [...store.cards(), repeatedCard]
                    });
                }
            },
            reviewCard(cardId: string, cardDifficulty: CardDifficulty) {
                patchState(store, { 
                    cardsStudied: [...store.cardsStudied(), { cardId, cardDifficulty }]
                });
            },
            saveReviewedCards: rxMethod(
                pipe(
                    tap(() => patchState(store, { loadingState: LoadingState.Loading })),
                    switchMap(() => {
                        return studyService.reviewCards(store.deckId(), store.cardsStudied()).pipe(
                            tap(() => {
                                snackBarService.open('Review Saved');
                                patchState(store, { loadingState: LoadingState.Complete })
                                
                                deckEvents.notifyReviewCompleted(store.deckId());
                                setTimeout(() => route.navigate(['/decks']), 3000);
                            }),
                            catchError(() => {
                                patchState(store, { loadingState: LoadingState.Error });
                                return of([]);
                            })
                        );
                    })
                )
            ),
        }),
    )
);