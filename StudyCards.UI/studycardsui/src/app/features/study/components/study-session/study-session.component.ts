import { ChangeDetectionStrategy, Component, inject, ViewChild } from '@angular/core';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute, Router } from '@angular/router';
import { StudyMethodology } from 'app/shared/models/study-methodology';
import { MatProgressBar } from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { CardsToStudyComponent } from "../cards-to-study/cards-to-study.component";
import { StudyCompleteComponent } from '../study-complete/study-complete.component';
import { CardDifficulty } from 'app/shared/models/card-difficulty';
import { StudySessionActionsComponent } from '../study-session-actions/study-session-actions.component';

@Component({
  selector: 'app-study-session',
  imports: [MatProgressBar, CardsToStudyComponent, StudyCompleteComponent, StudySessionActionsComponent],
  templateUrl: './study-session.component.html',
  styleUrl: './study-session.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionComponent {
  @ViewChild(StudySessionActionsComponent) studySessionActions!: StudySessionActionsComponent;

  readonly store = inject(StudyStore);
  private activatedRoute = inject(ActivatedRoute);
  private route = inject(Router)
  
  loadingState = LoadingState;

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    const methodology = this.activatedRoute.snapshot.paramMap.get('methodology') as keyof typeof StudyMethodology;

    this.store.loadCards({ deckId: deckId!, methodology: StudyMethodology[methodology] });
  }

  goBack(): void {
    this.route.navigate(['/decks']);
  }

  completeStudy(): void {
    this.studySessionActions.autoSave.clear();
    this.store.saveReviewedCards({});
  }

  reviewCard(cardId: string, difficulty: CardDifficulty): void {
    this.studySessionActions.autoSave.reset();
    this.store.reviewCard(cardId, difficulty);
  }

  repeatCard(cardId: string) {
    this.store.repeatCard(cardId);
  }
}
