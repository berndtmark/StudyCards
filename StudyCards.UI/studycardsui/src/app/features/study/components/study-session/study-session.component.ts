import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute, Router } from '@angular/router';
import { StudyMethodology } from 'app/shared/models/study-methodology';
import { MatProgressBar } from '@angular/material/progress-bar';
import { LoadingState } from 'app/shared/models/loading-state';
import { CardsToStudyComponent } from "../cards-to-study/cards-to-study.component";
import { MatIcon } from '@angular/material/icon';
import { MatMiniFabButton } from '@angular/material/button';
import { StudyCompleteComponent } from '../study-complete/study-complete.component';
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";

@Component({
  selector: 'app-study-session',
  imports: [MatProgressBar, CardsToStudyComponent, MatIcon, MatMiniFabButton, StudyCompleteComponent, BackNavComponent],
  templateUrl: './study-session.component.html',
  styleUrl: './study-session.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionComponent {
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
    this.store.saveReviewedCards({});
  }
}
