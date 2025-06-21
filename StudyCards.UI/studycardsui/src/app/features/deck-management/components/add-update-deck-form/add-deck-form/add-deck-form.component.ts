import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { DeckStore } from '../../../store/deck.store';
import { AddUpdateDeckBaseComponent } from '../add-update-deck-base.component';
import { BackNavComponent } from "../../../../../shared/components/back-nav/back-nav.component";

@Component({
  selector: 'app-add-deck-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, BackNavComponent],
  templateUrl: '../add-update-deck-form.component.html',
  styleUrl: '../add-update-deck-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckFormComponent extends AddUpdateDeckBaseComponent implements OnInit {
  readonly store = inject(DeckStore);

  saveButtonName = "Create Deck";

  ngOnInit(): void {
    this.initForm();
  }

  onSubmit(): void {
    if (this.deckForm.valid) {
      const result = this.formToDeck();
      this.store.addDeck(result);
      
      this.deckForm.reset();
      this.goBackToDeckList();
    }
  }
}
