import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckFormComponent } from "../deck-form/deck-form.component";
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";
import { Router } from '@angular/router';
import { Deck } from '../../models/deck';
import { DeckStore } from '../../store/deck.store';

@Component({
  selector: 'app-add-deck',
  imports: [DeckFormComponent, BackNavComponent],
  templateUrl: './add-deck.component.html',
  styleUrl: './add-deck.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckComponent {
  private router = inject(Router);
  readonly store = inject(DeckStore);

  goBackToDeckList(): void {
    this.router.navigate(['/decks']);
  }

  onSubmit(deck: Deck): void {
    this.store.addDeck(deck);
    this.goBackToDeckList();
  }
}
