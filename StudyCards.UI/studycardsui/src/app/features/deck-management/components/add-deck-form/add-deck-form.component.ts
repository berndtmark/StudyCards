import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { DeckStore } from '../../store/deck.store';

@Component({
  selector: 'app-add-deck-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './add-deck-form.component.html',
  styleUrl: './add-deck-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddDeckFormComponent {
  private router = inject(Router);
  readonly store = inject(DeckStore);
  deckForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.deckForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      maxReviews: [10, Validators.min(1)],
      maxNew: [5, Validators.min(1)]
    });
  }

  onSubmit(): void {
    if (this.deckForm.valid) {
      const deckForm = this.deckForm.value;

      this.store.addDeck({
        id: '3',
        name: deckForm.name,
        description: deckForm.description,
        cardCount: deckForm.maxReviews,
        lastModified: new Date()
      });
      
      this.deckForm.reset();
      this.goBackToDeckList();
    }
  }

  goBackToDeckList(): void {
    this.router.navigate(['/decks']);
  }
}
