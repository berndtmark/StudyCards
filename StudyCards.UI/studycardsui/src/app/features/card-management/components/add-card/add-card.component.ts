import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CardFormComponent } from "../card-form/card-form.component";
import { CardStore } from '../../store/card.store';
import { ActivatedRoute, Router } from '@angular/router';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";
import { LoadingState } from 'app/shared/models/loading-state';

@Component({
  selector: 'app-add-card',
  imports: [CardFormComponent, MyButtonComponent],
  templateUrl: './add-card.component.html',
  styleUrl: './add-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddCardComponent implements OnInit {
  readonly store = inject(CardStore);
  protected activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);

  loadingState = LoadingState;

  ngOnInit(): void {
    // ensure the deck cards is loaded before trying to get a card by id
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid') || '';
    this.store.loadDeckIfNot(deckId);
  }
  
  onCardSubmit(form: { cardFront: string; cardBack: string }): void {
    this.store.addCard({ 
        cardFront: form.cardFront, 
        cardBack: form.cardBack
    });
  }

  goBackToCardList(): void {
      const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
      this.router.navigate(['/cards', deckId]);
  }
}
