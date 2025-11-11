import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { DeckService as DeckServiceApi } from '../../../@api/services';
import { UpdateDeckRequest } from 'app/@api/models/update-deck-request';
import { Deck } from '../models/deck';
import { DateFuctions } from 'app/shared/functions/date-functions';
import { DeckResponse } from 'app/@api/models/deck-response';

@Injectable({
  providedIn: 'root'
})
export class DeckService {
  deckServiceApi = inject(DeckServiceApi);

  getDecks(): Observable<Deck[]> {
    return this.deckServiceApi.apiDeckGetdecksGet$Json().pipe(
      map(response => response.map(item => this.mapToDeck(item)))
    );
  }

  addDeck(deck: Deck): Observable<Deck> {
    const request: UpdateDeckRequest = { 
      deckName: deck.deckName,
      description: deck.description,
      newCardsPerDay: deck.deckSettings?.newCardsPerDay,
      reviewsPerDay: deck.deckSettings?.reviewsPerDay
    }

    return this.deckServiceApi.apiDeckAdddeckPost$Json({ body: request }).pipe(
      map(response => this.mapToDeck(response))
    );
  }

  updateDeck(deck: Deck): Observable<Deck> {
    const request: UpdateDeckRequest = { 
      deckId: deck.id,
      deckName: deck.deckName,
      description: deck.description,
      newCardsPerDay: deck.deckSettings?.newCardsPerDay,
      reviewsPerDay: deck.deckSettings?.reviewsPerDay
    }

    return this.deckServiceApi.apiDeckUpdatedeckPut$Json({ body: request }).pipe(
      map(response => this.mapToDeck(response))
    );
  }

  removeDeck(deckId: string): Observable<boolean> {
    return this.deckServiceApi.apiDeckRemovedeckDeckIdDelete$Json({ deckId });
  }

  static hasReviewsToday(deck: Deck, reviews: number): boolean {
    let reviewsToday = reviews;

    if (deck.deckReviewStatus?.lastReview) {
      reviewsToday += DateFuctions.isToday(new Date(deck.deckReviewStatus!.lastReview!)) ? deck.deckReviewStatus!.reviewCount! : 0;
    }

    return reviewsToday < Math.min(deck.deckSettings!.reviewsPerDay!, deck.cardCount!)
  }

  private mapToDeck(response: DeckResponse): Deck {
    return {
      id: response.id,
      deckName: response.deckName,
      description: response.description,
      cardCount: response.cardCount ? Number(response.cardCount) : undefined,
      deckSettings: {
        newCardsPerDay: response.deckSettings?.newCardsPerDay ? Number(response.deckSettings.newCardsPerDay) : undefined,
        reviewsPerDay: response.deckSettings?.reviewsPerDay ? Number(response.deckSettings.reviewsPerDay) : undefined
      },
      deckReviewStatus: {
        lastReview: response.deckReviewStatus?.lastReview,
        reviewCount: response.deckReviewStatus?.reviewCount ? Number(response.deckReviewStatus.reviewCount) : undefined
      }
    };
  }
}
