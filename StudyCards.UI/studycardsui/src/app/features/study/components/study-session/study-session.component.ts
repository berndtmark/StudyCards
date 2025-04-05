import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { StudyCardComponent } from '../study-card/study-card.component';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute, Router } from '@angular/router';
import { StudyMethodology } from 'app/shared/models/study-methodology';
import { MatProgressBar } from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { NgIf } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';

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
  
  currentCardIndex = 0;
  loadingState = LoadingState;

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    const methodology = this.activatedRoute.snapshot.paramMap.get('methodology') as keyof typeof StudyMethodology;

    this.store.loadCards({ deckId: deckId!, methodology: StudyMethodology[methodology] });
  }

  onCardStudied() {
    this.currentCardIndex++;
  }

  goBack(): void {
    this.route.navigate(['/decks']);
  }
}
