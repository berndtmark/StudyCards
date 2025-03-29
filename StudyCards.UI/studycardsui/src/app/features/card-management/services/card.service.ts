import { inject, Injectable } from '@angular/core';
import { Card } from 'app/@api/models/card';
import { Observable } from 'rxjs';
import { CardService as CardServiceApi } from '../../../@api/services';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  cardServiceApi = inject(CardServiceApi);

  getCards(deckId: string): Observable<Card[]> {
    return this.cardServiceApi.apiCardGetcardsGet$Json({ deckId });
  }

  addCard(deckId: string, cardFront: string, cardBack: string): Observable<Card> {
    return this.cardServiceApi.apiCardAddcardPost$Json({ body: { deckId, cardFront, cardBack } });
  }

  updateCard(deckId: string, cardId: string, cardFront: string, cardBack: string): Observable<Card> {
    return this.cardServiceApi.apiCardUpdatecardPut$Json({ body: { cardId, deckId, cardFront, cardBack } });
  }
}
