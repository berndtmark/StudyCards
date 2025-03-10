import { inject, Injectable } from '@angular/core';
import { map, Observable, of, pipe } from 'rxjs';
import { Deck } from '../models/deck.model';
import { DeckService as DeckServiceApi } from '../../../@api/services';

@Injectable({
  providedIn: 'root'
})
export class DeckService {
  deckServiceApi = inject(DeckServiceApi);

  getDecks(): Observable<Deck[]> {
    // todo: this is mostly a placeholder implementation
    const decks = this.deckServiceApi.apiDeckGetdecksGet$Json()
    .pipe(
      map((decks) =>
        decks.map((deck) => ({
          id: deck.id,
          name: deck.deckName,
          description: 'No Description',
          cardCount: 25,
          lastModified: new Date(deck.updatedDate!)
        }) as Deck)
      )
    );

    return decks;
  }
}
