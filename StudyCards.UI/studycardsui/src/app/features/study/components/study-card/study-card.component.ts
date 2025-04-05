import { NgIf } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-study-card',
  imports: [NgIf],
  templateUrl: './study-card.component.html',
  styleUrls: ['./study-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyCardComponent {
  @Input() frontText: string = '';
  @Input() backText: string = '';
  @Output() cardStudied = new EventEmitter<void>();
  
  isFlipped: boolean = false;
  noAnimation: boolean = false;

  flipCard(): void {
    this.isFlipped = !this.isFlipped;
  }

  onCardCompleted() {
    this.isFlipped = false;
    this.noAnimation = true;
    this.cardStudied.emit();

    // Reset noAnimation after a brief delay to allow the state to update
    setTimeout(() => {
      this.noAnimation = false;
    }, 50);
  }
}
