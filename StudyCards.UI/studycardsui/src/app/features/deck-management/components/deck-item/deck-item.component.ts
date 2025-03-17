import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, Input, OnDestroy } from '@angular/core';
import { Deck } from 'app/@api/models/deck';
import { DeckActionsComponent } from '../deck-actions/deck-actions.component';
import { Router } from '@angular/router';
import { DeckStore } from '../../store/deck.store';
import { DialogService } from 'app/shared/services/dialog.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-deck-item',
  imports: [CommonModule, DeckActionsComponent],
  templateUrl: './deck-item.component.html',
  styleUrl: './deck-item.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckItemComponent implements OnDestroy {
  private router = inject(Router);
  private dialogService = inject(DialogService);
  readonly store = inject(DeckStore);

  private unsubscribe$ = new Subject<void>();

  @Input() deck!: Deck;

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onDeckSelected(id: string): void {
    console.log(`Deck selected: ${id}`);
  }

  editDeck(id: string): void {
    this.router.navigate(['/decks/edit', id]);
  }

  removeDeck(id: string): void {
    this.dialogService.confirm('Delete Deck', 'Are you sure you want to remove this deck?', 'Yes')
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => this.store.removeDeck(id));
  }
}
