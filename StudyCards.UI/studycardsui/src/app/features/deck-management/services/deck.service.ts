import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Deck } from '../models/deck.model';

@Injectable({
  providedIn: 'root'
})
export class DeckService {
  // Temporary mock data
  getDecks(): Observable<Deck[]> {
    return of([
      {
        id: '1',
        name: 'JavaScript Basics',
        description: 'Fundamental concepts of JavaScript',
        cardCount: 25,
        lastModified: new Date()
      },
      {
        id: '2',
        name: 'JavaScript Basics 2',
        description: 'Fundamental concepts of JavaScript',
        cardCount: 25,
        lastModified: new Date()
      }
    ]);
  }
}
