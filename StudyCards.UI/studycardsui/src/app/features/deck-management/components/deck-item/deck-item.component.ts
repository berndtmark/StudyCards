import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Deck } from 'app/@api/models/deck';
import { DeckActionsComponent } from '../deck-actions/deck-actions.component';

@Component({
  selector: 'app-deck-item',
  imports: [CommonModule, DeckActionsComponent],
  templateUrl: './deck-item.component.html',
  styleUrl: './deck-item.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckItemComponent {
  @Input() deck!: Deck;

  onDeckSelected(id: string): void {
    console.log(`Deck selected: ${id}`);
  }

  editDeck(id: string): void {
    console.log(`Deck edited: ${id}`);
  }

  removeDeck(id: string): void {
    console.log(`Deck removed: ${id}`);
  }
}
