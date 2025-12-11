import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject, input, computed } from '@angular/core';
import { Router } from '@angular/router';
import { DeckStore } from '../../store/deck.store';
import { Deck } from '../../models/deck';
import { StatusBadgeComponent } from '../../../../shared/components/status-badge/status-badge.component';

@Component({
  selector: 'app-deck-item',
  imports: [CommonModule, StatusBadgeComponent],
  templateUrl: './deck-item.component.html',
  styleUrl: './deck-item.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckItemComponent {
  private router = inject(Router);
  readonly store = inject(DeckStore);

  deck = input.required<Deck>();

  reviewsToday = computed(() => this.deck().hasReviewsToday || false);

  onDeckSelected(id: string): void {
    this.router.navigate(['/study', id]);
  }
}
