import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { StudyCardActionsComponent } from "../study-card-actions/study-card-actions.component";
import { CardDifficulty } from 'app/shared/models/card-difficulty';

@Component({
  selector: 'app-study-card',
  imports: [NgIf, StudyCardActionsComponent],
  templateUrl: './study-card.component.html',
  styleUrls: ['./study-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyCardComponent {
  @Input() cardId!: string;
  @Input() frontText: string = '';
  @Input() backText: string = '';
  @Output() cardStudied = new EventEmitter<void>();
  
  isFlipped: boolean = false;
  noAnimation: boolean = false;

  flipCard(): void {
    this.isFlipped = !this.isFlipped;
  }

  onCardCompleted(cardDifficulty: CardDifficulty): void {
    this.reviewCard(cardDifficulty);

    this.isFlipped = false;
    this.noAnimation = true;
    this.cardStudied.emit();

    // Reset noAnimation after a brief delay to allow the state to update
    setTimeout(() => {
      this.noAnimation = false;
    }, 50);
  }

  reviewCard(cardDifficulty: CardDifficulty): void {
    // todo
    console.log(`Card ${this.cardId} reviewed with difficulty:`, cardDifficulty);
  }
}
