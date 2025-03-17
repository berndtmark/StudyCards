import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, Input } from '@angular/core';
import { Deck } from 'app/@api/models/deck';
import { DeckActionsComponent } from '../deck-actions/deck-actions.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-deck-item',
  imports: [CommonModule, DeckActionsComponent],
  templateUrl: './deck-item.component.html',
  styleUrl: './deck-item.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckItemComponent {
  private router = inject(Router);

  @Input() deck!: Deck;

  onDeckSelected(id: string): void {
    console.log(`Deck selected: ${id}`);
  }

  editDeck(id: string): void {
    this.router.navigate(['/decks/edit', id]);
  }

  removeDeck(id: string): void {
    console.log(`Deck removed: ${id}`);
  }
}
