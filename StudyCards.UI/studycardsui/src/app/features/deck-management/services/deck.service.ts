import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DeckService as DeckServiceApi } from '../../../@api/services';
import { DeckResponse } from 'app/@api/models/deck-response';
import { UpdateDeckRequest } from 'app/@api/models/update-deck-request';

@Injectable({
  providedIn: 'root'
})
export class DeckService {
  deckServiceApi = inject(DeckServiceApi);

  getDecks(): Observable<DeckResponse[]> {
    return this.deckServiceApi.apiDeckGetdecksGet$Json();
  }

  addDeck(deck: DeckResponse): Observable<DeckResponse> {
    const request: UpdateDeckRequest = { 
      deckName: deck.deckName,
      description: deck.description,
      newCardsPerDay: deck.deckSettings?.newCardsPerDay,
      reviewsPerDay: deck.deckSettings?.reviewsPerDay
    }

    return this.deckServiceApi.apiDeckAdddeckPost$Json({ body: request })
  }

  updateDeck(deck: DeckResponse): Observable<DeckResponse> {
    const request: UpdateDeckRequest = { 
      deckId: deck.id,
      deckName: deck.deckName,
      description: deck.description,
      newCardsPerDay: deck.deckSettings?.newCardsPerDay,
      reviewsPerDay: deck.deckSettings?.reviewsPerDay
    }

    return this.deckServiceApi.apiDeckUpdatedeckPut$Json({ body: request });
  }

  removeDeck(deckId: string): Observable<boolean> {
    return this.deckServiceApi.apiDeckRemovedeckDeckIdDelete$Json({ deckId });
  }
}
