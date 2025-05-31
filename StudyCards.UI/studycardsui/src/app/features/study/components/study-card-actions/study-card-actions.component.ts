import { ChangeDetectionStrategy, Component, output } from '@angular/core';
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
  reviewedCard = output<CardDifficulty>();

  cardDifficulty = CardDifficulty;
}
