import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckStore } from '../../store/deck.store';
import { AddDeckComponent } from "../add-deck/add-deck.component";
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { Router } from '@angular/router';
import { DeckItemComponent } from '../deck-item/deck-item.component';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-deck-list',
  imports: [AddDeckComponent, MatProgressBarModule, DeckItemComponent, NgIf],
  templateUrl: './deck-list.component.html',
  styleUrl: './deck-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckListComponent {
  readonly store = inject(DeckStore);
  private router = inject(Router);

  loadingState = LoadingState;

  addDeck(): void {
    this.router.navigate(['/decks/add']);
  }
}
