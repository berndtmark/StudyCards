import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { DeckFormComponent } from "../deck-form/deck-form.component";
import { Router } from '@angular/router';
import { Deck } from '../../models/deck';
import { DeckStore } from '../../store/deck.store';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";

@Component({
  selector: 'app-add-deck',
  imports: [DeckFormComponent, MyButtonComponent],
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
