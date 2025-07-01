import { inject, Injectable } from '@angular/core';
import { StudyService as StudyServiceApi } from '../../../@api/services';
import { StudyMethodology } from '../../../shared/models/study-methodology';
import { CardDifficulty } from 'app/shared/models/card-difficulty';
import { CardDifficulty as CD } from 'app/@api/models/card-difficulty';

@Injectable({
  providedIn: 'root'
})
export class StudyService {
  private studyServiceApi = inject(StudyServiceApi);

  getStudyCards(deckId: string, methodology: StudyMethodology) {
    return this.studyServiceApi.getstudycard$Json({ methodology, deckId });
  }

  reviewCards(deckId: string, cards: { cardId: string; cardDifficulty: CardDifficulty }[]) {
    const allRepeatedCards = cards.filter(card => card.cardDifficulty === CardDifficulty.Repeat);

    const reviewedCards: { cardId: string, cardDifficulty: CD, repeatCount: number }[] = cards.
      filter(card => card.cardDifficulty !== CardDifficulty.Repeat).
      map(card => {
        const repeatedCards = allRepeatedCards.filter(rp => card.cardId == rp.cardId)?.length ?? 0;

        return {
          cardId: card.cardId,
          repeatCount: repeatedCards,
          cardDifficulty: card.cardDifficulty
        };
      }
    );

    return this.studyServiceApi.reviewcards$Json({ body: { deckId, cards: reviewedCards } });
  }
}
