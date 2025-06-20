import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, input, OnDestroy, computed } from '@angular/core';
import { DeckResponse } from 'app/@api/models/deck-response';
import { DeckActionsComponent } from '../deck-actions/deck-actions.component';
import { Router } from '@angular/router';
import { DeckStore } from '../../store/deck.store';
import { DialogService } from 'app/shared/services/dialog.service';
import { Subject, takeUntil } from 'rxjs';
import { StatusBadgeComponent } from 'app/shared/components/status-badge/status-badge.component';
import { DateFuctions } from 'app/shared/functions/date-functions';

@Component({
  selector: 'app-deck-item',
  imports: [CommonModule, DeckActionsComponent, StatusBadgeComponent],
  templateUrl: './deck-item.component.html',
  styleUrl: './deck-item.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckItemComponent implements OnDestroy {
  private router = inject(Router);
  private dialogService = inject(DialogService);
  readonly store = inject(DeckStore);

  private unsubscribe$ = new Subject<void>();

  deck = input.required<DeckResponse>();

  reviewsToday = computed(() => DateFuctions.isToday(new Date(this.deck().deckReviewStatus?.lastReview || 0)));

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  onDeckSelected(id: string): void {
    this.router.navigate(['/study', id]);
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
