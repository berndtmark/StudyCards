import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckStore } from '../../store/deck.store';

@Component({
  selector: 'app-add-deck',
  imports: [],
  templateUrl: './add-deck.component.html',
  styleUrl: './add-deck.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckComponent {
  readonly store = inject(DeckStore);

  addDeck(): void {
    // just a sample
    this.store.addDeck({
      id: '3',
      name: 'My New Deck Item',
      description: 'Fundamental concepts of Life',
      cardCount: 0,
      lastModified: new Date()
    });
  }
}
