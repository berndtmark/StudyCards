import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeckEventsService {
  private reviewCompletedSignal = signal<string | null>(null);

  get reviewCompleted() {
    return this.reviewCompletedSignal.asReadonly();
  }

  // todo add count of reviews today
  notifyReviewCompleted(deckId: string, reviewCount: number): void {
    this.reviewCompletedSignal.set(deckId);
  }
}