import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CardStore } from '../../store/card.store';
import { ActivatedRoute, Router } from '@angular/router';
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";
import { CardFormComponent } from "../card-form/card-form.component";
import { DialogService } from 'app/shared/services/dialog.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-update-card',
  imports: [BackNavComponent, CardFormComponent, MatButtonModule],
  templateUrl: './update-card.component.html',
  styleUrl: './update-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpdateCardComponent implements OnInit {
  readonly store = inject(CardStore);
  protected activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private dialogService = inject(DialogService);

  cardId!: string;

  ngOnInit(): void {
    // ensure the deck cards is loaded before trying to get a card by id
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid') || '';
    if (!this.store.deckLoaded(deckId))
        this.store.loadCards(deckId);

    this.cardId = this.activatedRoute.snapshot.paramMap.get('cardid')!;
  }

  onCardSubmit(form: { cardFront: string; cardBack: string }): void {
    this.store.updateCard({ 
        cardId: this.cardId!,
        cardFront: form.cardFront, 
        cardBack: form.cardBack,
    });

    this.goBackToCardList();
  }

  goBackToCardList(): void {
      const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
      this.router.navigate(['/cards', deckId]);
  }

  onRemovedCard(): void {
    this.dialogService.confirm('Delete Card', 'Are you sure you want to remove this Card?', 'Yes')
        .subscribe(() => {
            this.store.removeCard(this.cardId!);
            this.goBackToCardList();
        });
  }
}
