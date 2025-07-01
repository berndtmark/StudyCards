import { ChangeDetectionStrategy, Component, output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-deck-actions',
  imports: [MatIcon],
  templateUrl: './deck-actions.component.html',
  styleUrl: './deck-actions.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckActionsComponent {
  edit = output<void>();
  remove = output<void>();
}
