import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeckEventsService {
  private reviewCompletedSignal = signal<string | null>(null);

  get reviewCompleted() {
    return this.reviewCompletedSignal.asReadonly();
  }

  notifyReviewCompleted(deckId: string) {
    this.reviewCompletedSignal.set(deckId);
  }
}