import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { DeckStore } from 'app/features/deck-management/store/deck.store';
import { AddUpdateDeckBaseComponent } from '../add-update-deck-base.component';

@Component({
  selector: 'app-update-deck-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: '../add-update-deck-form.component.html',
  styleUrl: '../add-update-deck-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpdateDeckFormComponent extends AddUpdateDeckBaseComponent {
  private route = inject(ActivatedRoute);
  readonly store = inject(DeckStore);

  saveButtonName = "Update Deck";

  constructor() {
    super();

    effect(() => {
      const deckId = this.route.snapshot.paramMap.get('deckid') || '';
      this.patchFormValues(deckId);
    });
  }

  onSubmit(): void {
    if (this.deckForm.valid) {
      this.store.updateDeck();
      
      this.deckForm.reset();
      this.goBackToDeckList();
    }
  }

  private patchFormValues(deckId: string): void {
    const deck = this.store.getDeckById(deckId);

    if (deck) {
      var result = this.deckToForm(deck!);
      this.deckForm.patchValue(result);
    }
  }
}
