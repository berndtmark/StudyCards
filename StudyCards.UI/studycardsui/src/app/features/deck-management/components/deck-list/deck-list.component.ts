import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckStore } from '../../store/deck.store';
import { CommonModule } from '@angular/common';
import { AddDeckComponent } from "../add-deck/add-deck.component";

@Component({
  selector: 'app-deck-list',
  imports: [CommonModule, AddDeckComponent],
  providers: [DeckStore],
  templateUrl: './deck-list.component.html',
  styleUrl: './deck-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckListComponent {
  readonly store = inject(DeckStore);

  onDeckSelected(id: string): void {
    console.log(`Deck selected: ${id}`);
  }
}
