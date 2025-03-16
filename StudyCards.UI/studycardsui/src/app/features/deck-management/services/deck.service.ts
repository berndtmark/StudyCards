import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DeckService as DeckServiceApi } from '../../../@api/services';
import { Deck } from 'app/@api/models/deck';

@Injectable({
  providedIn: 'root'
})
export class DeckService {
  deckServiceApi = inject(DeckServiceApi);

  getDecks(): Observable<Deck[]> {
    return this.deckServiceApi.apiDeckGetdecksGet$Json();
  }

  addDeck(deck: Deck): Observable<Deck> {
    return this.deckServiceApi.apiDeckAdddeckPost$Json({ body: { deckName: deck.deckName } })
  }
}
