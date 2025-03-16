import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckStore } from '../../store/deck.store';
import { CommonModule } from '@angular/common';
import { AddDeckComponent } from "../add-deck/add-deck.component";
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { Router } from '@angular/router';

@Component({
  selector: 'app-deck-list',
  imports: [CommonModule, AddDeckComponent, MatProgressBarModule],
  templateUrl: './deck-list.component.html',
  styleUrl: './deck-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckListComponent {
  readonly store = inject(DeckStore);
  private router = inject(Router);

  loadingState = LoadingState;

  onDeckSelected(id: string): void {
    console.log(`Deck selected: ${id}`);
  }

  addDeck(): void {
    this.router.navigate(['/decks/add']);
  }
}
