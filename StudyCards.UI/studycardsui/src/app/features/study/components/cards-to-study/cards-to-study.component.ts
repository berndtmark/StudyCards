import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
import { CardDifficulty } from 'app/shared/models/card-difficulty';
import { StudyStore } from '../../store/study.store';
import { StudyCardComponent } from "../study-card/study-card.component";
import { CardResponse } from 'app/@api/models/card-response';

@Component({
  selector: 'app-cards-to-study',
  imports: [StudyCardComponent],
  templateUrl: './cards-to-study.component.html',
  styleUrl: './cards-to-study.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardsToStudyComponent {
  cardsToStudy = input<CardResponse[]>([]);
  completeStudy = output<void>();

  readonly store = inject(StudyStore);

  private currentIndex = signal(0);
  readonly cards = computed(() => this.cardsToStudy());
  readonly currentCard = computed(() => this.cards()[this.currentIndex()]);
  readonly cardsLeft = computed(() => `Cards Studied: ${this.currentIndex()} Cards Left: ${this.cards().length - this.currentIndex()}`);
  readonly isComplete = computed(() => this.currentIndex() >= this.cards().length);

  onCardStudied(card: [string, CardDifficulty]): void {
    if (card[1] === CardDifficulty.Repeat) {
      this.store.repeatCard(card[0]);
    }

    this.store.reviewCard(card[0], card[1]);
    this.currentIndex.update(i => ++i);

    if (this.isComplete()) {
      this.completeStudy.emit();
    }
  }
}
