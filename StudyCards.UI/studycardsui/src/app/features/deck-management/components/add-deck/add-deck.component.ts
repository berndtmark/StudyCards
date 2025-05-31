import { ChangeDetectionStrategy, Component, output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-add-deck',
  imports: [MatIcon],
  templateUrl: './add-deck.component.html',
  styleUrl: './add-deck.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckComponent {
  addDeck = output<void>();
}
