import { ChangeDetectionStrategy, Component, DestroyRef, inject, OnDestroy, OnInit } from '@angular/core';
import { DeckFormComponent } from "../deck-form/deck-form.component";
import { DeckStore } from '../../store/deck.store';
import { ActivatedRoute, Router } from '@angular/router';
import { Deck } from '../../models/deck';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { MatButtonModule } from '@angular/material/button';
import { DialogService } from '../../../../shared/services/dialog.service';

@Component({
  selector: 'app-update-deck',
  imports: [DeckFormComponent, MyButtonComponent, MatButtonModule],
  templateUrl: './update-deck.component.html',
  styleUrl: './update-deck.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpdateDeckComponent implements OnInit {
  private router = inject(Router);
  readonly store = inject(DeckStore);
  private route = inject(ActivatedRoute);
  private dialogService = inject(DialogService);
  
  private readonly destroyRef = inject(DestroyRef);

  deckId!: string;

  ngOnInit(): void {
    this.deckId = this.route.snapshot.paramMap.get('deckid') || '';
  }

  goBackToDeckList(): void {
      this.router.navigate(['/decks']);
  }

  onSubmit(deck: Deck): void {
    this.store.updateDeck(deck);
    this.goBackToDeckList();
  }

  removeDeck(): void {
    this.dialogService.confirm('Delete Deck', 'Are you sure you want to remove this deck?', 'Yes')
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => {
        this.store.removeDeck(this.deckId); 
        this.goBackToDeckList();
      });
  }
}
