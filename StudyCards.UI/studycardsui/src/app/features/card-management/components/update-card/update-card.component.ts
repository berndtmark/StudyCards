import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { CardStore } from '../../store/card.store';
import { ActivatedRoute, Router } from '@angular/router';
import { CardFormComponent } from "../card-form/card-form.component";
import { MatButtonModule } from '@angular/material/button';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";
import { DialogService } from '../../../../shared/services/dialog.service';

@Component({
  selector: 'app-update-card',
  imports: [CardFormComponent, MatButtonModule, MyButtonComponent],
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
    this.store.loadDeckIfNot(deckId);

    this.cardId = this.activatedRoute.snapshot.paramMap.get('cardid')!;
  }

  onCardSubmit(form: { cardFront: string; cardBack: string }): void {
    this.store.updateCard({ 
        cardId: this.cardId!,
        cardFront: form.cardFront, 
        cardBack: form.cardBack,
    });
  }

  goBackToCardList(): void {
      const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
      this.router.navigate(['/cards', deckId]);
  }

  onRemovedCard(): void {
    this.dialogService.confirm('Delete Card', 'Are you sure you want to remove this Card?', 'Yes')
        .subscribe(() => {
            this.store.removeCard(this.cardId!);
        });
  }
}
