import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { StudyStore } from '../../store/study.store';
import { ActivatedRoute } from '@angular/router';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-study-landing',
  imports: [MatIcon],
  templateUrl: './study-landing.component.html',
  styleUrl: './study-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyLandingComponent implements OnInit {
  readonly store = inject(StudyStore);
  private activatedRoute = inject(ActivatedRoute)

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    
    if (this.store.deckId() !== deckId)
     this.store.init(deckId!);
  }
}
