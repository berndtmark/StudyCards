import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { StudyMethodology } from '../../../../shared/models/study-methodology';

@Component({
  selector: 'app-study-landing',
  imports: [MatIcon],
  templateUrl: './study-landing.component.html',
  styleUrl: './study-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyLandingComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute)
  private router = inject(Router);

  deckId!: string | null;
  methodology = StudyMethodology;

  ngOnInit(): void {
    this.deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
  }

  manageCards(): void {
    this.router.navigate(['/cards', this.deckId]);
  }

  manageDeck(): void {
    this.router.navigate(['/decks/edit', this.deckId]);
  }

  studySession(methodology: StudyMethodology): void {
    this.router.navigate(['session', methodology], { 
      relativeTo: this.activatedRoute 
    });
  }
}
