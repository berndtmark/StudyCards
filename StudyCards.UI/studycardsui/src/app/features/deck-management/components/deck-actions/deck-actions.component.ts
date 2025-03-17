import { ChangeDetectionStrategy, Component, EventEmitter, Output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-deck-actions',
  imports: [MatIcon],
  templateUrl: './deck-actions.component.html',
  styleUrl: './deck-actions.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckActionsComponent {
  @Output() edit = new EventEmitter<void>();
  @Output() remove = new EventEmitter<void>();
}
