import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { CardDifficulty } from 'app/shared/models/card-difficulty';

@Component({
  selector: 'app-study-card-actions',
  imports: [MatButton],
  templateUrl: './study-card-actions.component.html',
  styleUrl: './study-card-actions.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyCardActionsComponent {
  @Output() reviewedCard = new EventEmitter<CardDifficulty>();

  cardDifficulty = CardDifficulty;
}
