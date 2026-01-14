import { ChangeDetectionStrategy, Component, computed, inject, input, output, signal } from '@angular/core';
import { StudyCardComponent } from "../study-card/study-card.component";
import { CardResponse } from '../../../../@api/models/card-response';
import { CardDifficulty } from '../../../../shared/models/card-difficulty';

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
  reviewCard = output<{cardId: string, dificulty: CardDifficulty}>();
  repeatCard = output<string>();

  private currentIndex = signal(0);
  readonly cards = computed(() => this.cardsToStudy());
  readonly currentCard = computed(() => this.cards()[this.currentIndex()]);
  readonly cardsLeft = computed(() => `Cards Studied: ${this.currentIndex()} Cards Left: ${this.cards().length - this.currentIndex()}`);
  readonly isComplete = computed(() => this.currentIndex() >= this.cards().length);

  onCardStudied(card: [string, CardDifficulty]): void {
    const isRepeat = card[1] === CardDifficulty.Repeat;
    if (isRepeat) {
      this.repeatCard.emit(card[0]);
    }

    this.reviewCard.emit({cardId: card[0], dificulty: card[1]});
    this.currentIndex.update(i => ++i);

    if (!isRepeat && this.isComplete()) {
      this.completeStudy.emit();
    }
  }
}
