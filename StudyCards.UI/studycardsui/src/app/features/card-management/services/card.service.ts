import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CardService as CardServiceApi } from '../../../@api/services';
import { CardResponse } from 'app/@api/models/card-response';
import { CardText } from 'app/@api/models/card-text';
import { AddCardsResponse } from 'app/@api/models/add-cards-response';
import { ImportCard } from '../models/import-card';
import { PagedResultOfCardResponse } from 'app/@api/models/paged-result-of-card-response';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  cardServiceApi = inject(CardServiceApi);

  getCards(deckId: string, pageNumber: number, pageSize: number, searchTerm?: string): Observable<PagedResultOfCardResponse> {
    return this.cardServiceApi.apiCardGetcardsGet$Json({ deckId, pageNumber, pageSize, searchTerm });
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

  addCards(deckId: string, cards: ImportCard[]): Observable<AddCardsResponse> {
    const cardTexts: CardText[] = cards.map(c => ({ cardFront: c.cardFront!, cardBack: c.cardBack! }));
    return this.cardServiceApi.apiCardAddcardsPost$Json({ body: { deckId, cards: cardTexts } });
  }
}
