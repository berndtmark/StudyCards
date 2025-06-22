import { Injectable, signal } from '@angular/core';

type ReviewCompletedPayload = {
  deckId: string;
  reviewCount: number;
};

@Injectable({
  providedIn: 'root'
})
export class DeckEventsService {
  private reviewCompletedSignal = signal<ReviewCompletedPayload>({} as ReviewCompletedPayload);

  get reviewCompleted() {
    return this.reviewCompletedSignal.asReadonly();
  }

  notifyReviewCompleted(deckId: string, reviewCount: number): void {
    this.reviewCompletedSignal.set({ deckId, reviewCount });
  }
}