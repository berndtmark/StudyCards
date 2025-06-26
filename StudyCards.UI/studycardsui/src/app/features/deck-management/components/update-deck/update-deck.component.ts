import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";
import { DeckFormComponent } from "../deck-form/deck-form.component";
import { DeckStore } from '../../store/deck.store';
import { ActivatedRoute, Router } from '@angular/router';
import { Deck } from '../../models/deck';

@Component({
  selector: 'app-update-deck',
  imports: [BackNavComponent, DeckFormComponent],
  templateUrl: './update-deck.component.html',
  styleUrl: './update-deck.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpdateDeckComponent implements OnInit {
  private router = inject(Router);
  readonly store = inject(DeckStore);
  private route = inject(ActivatedRoute);

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
}
