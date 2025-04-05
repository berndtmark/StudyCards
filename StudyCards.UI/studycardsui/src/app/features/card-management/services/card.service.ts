import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CardService as CardServiceApi } from '../../../@api/services';
import { CardResponse } from 'app/@api/models/card-response';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  cardServiceApi = inject(CardServiceApi);

  getCards(deckId: string): Observable<CardResponse[]> {
    return this.cardServiceApi.apiCardGetcardsGet$Json({ deckId });
  }

  addCard(deckId: string, cardFront: string, cardBack: string): Observable<CardResponse> {
    return this.cardServiceApi.apiCardAddcardPost$Json({ body: { deckId, cardFront, cardBack } });
  }

  updateCard(deckId: string, cardId: string, cardFront: string, cardBack: string): Observable<CardResponse> {
    return this.cardServiceApi.apiCardUpdatecardPut$Json({ body: { cardId, deckId, cardFront, cardBack } });
  }

  removeCard(deckId: string, cardId: string): Observable<boolean> {
    return this.cardServiceApi.removecard$Json({ deckId, cardId });
  }
}
