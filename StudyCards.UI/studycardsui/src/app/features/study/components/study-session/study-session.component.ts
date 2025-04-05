import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { StudyCardComponent } from '../study-card/study-card.component';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute } from '@angular/router';
import { StudyMethodology } from 'app/shared/models/study-methodology';
import { MatProgressBar } from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-study-session',
  imports: [StudyCardComponent, MatProgressBar, NgIf],
  templateUrl: './study-session.component.html',
  styleUrl: './study-session.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionComponent {
  readonly store = inject(StudyStore);
  activatedRoute = inject(ActivatedRoute);
  
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
}
