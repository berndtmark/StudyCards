import { ChangeDetectionStrategy, Component, computed, inject, signal, WritableSignal } from '@angular/core';
import { StudyCardComponent } from '../study-card/study-card.component';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute, Router } from '@angular/router';
import { StudyMethodology } from 'app/shared/models/study-methodology';
import { MatProgressBar } from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { NgIf } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { CardDifficulty } from 'app/shared/models/card-difficulty';

@Component({
  selector: 'app-study-session',
  imports: [StudyCardComponent, MatProgressBar, NgIf, MatIcon, MatButton],
  templateUrl: './study-session.component.html',
  styleUrl: './study-session.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionComponent {
  readonly store = inject(StudyStore);
  private activatedRoute = inject(ActivatedRoute);
  private route = inject(Router)
  
  private currentIndex = signal(0);
  readonly cards = computed(() => this.store.cards());
  readonly currentCard = computed(() => this.cards()[this.currentIndex()]);
  readonly cardsLeft = computed(() => `Cards Studied: ${this.currentIndex()} Cards Left: ${this.cards().length - this.currentIndex()}`);
  readonly isComplete = computed(() => this.currentIndex() >= this.cards().length);
  
  loadingState = LoadingState;

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    const methodology = this.activatedRoute.snapshot.paramMap.get('methodology') as keyof typeof StudyMethodology;

    this.store.loadCards({ deckId: deckId!, methodology: StudyMethodology[methodology] });
  }

  onCardStudied(card: [string, CardDifficulty]): void {
    if (card[1] === CardDifficulty.Repeat) {
      this.store.repeatCard(card[0]);
    }

    this.store.reviewCard(card[0], card[1]);
    this.currentIndex.update(i => ++i);
  }

  goBack(): void {
    this.route.navigate(['/decks']);
  }

  completeStudy(): void {
    this.store.saveReviewedCards({});
  }
}
