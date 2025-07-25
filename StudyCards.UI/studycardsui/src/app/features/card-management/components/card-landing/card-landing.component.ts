import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CardStore } from '../../store/card.store';
import { CardListComponent } from "../card-list/card-list.component";
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingState } from 'app/shared/models/loading-state';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";

@Component({
  selector: 'app-card-landing',
  imports: [CardListComponent, MatProgressBar, MatButtonModule, MatIconModule, MyButtonComponent],
  templateUrl: './card-landing.component.html',
  styleUrl: './card-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardLandingComponent implements OnInit {
  readonly store = inject(CardStore);
  activatedRoute = inject(ActivatedRoute)
  private router = inject(Router);

  loadingState = LoadingState;

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    this.store.loadDeckIfNot(deckId!);
  }

  goBack(): void {
    this.router.navigate(['/decks']);
  }

  addCard(): void {
    this.router.navigate(['add'], { 
      relativeTo: this.activatedRoute 
    });
  }

  updateCard(cardId: string): void {
    this.router.navigate(['edit', cardId], { 
      relativeTo: this.activatedRoute 
    });
  }

  import(): void {
    this.router.navigate(['import'], { 
      relativeTo: this.activatedRoute 
    });
  }

  export(): void {
    this.store.export();
  }
}
