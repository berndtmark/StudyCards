import { ChangeDetectionStrategy, Component, output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-add-deck',
  imports: [MatIcon],
  templateUrl: './add-deck-button.component.html',
  styleUrl: './add-deck-button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckButtonComponent {
  addDeck = output<void>();
}
