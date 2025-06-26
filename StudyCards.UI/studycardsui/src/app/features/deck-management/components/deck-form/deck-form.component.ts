import { ChangeDetectionStrategy, Component, inject, input, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Deck } from '../../models/deck';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-deck-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './deck-form.component.html',
  styleUrl: './deck-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckFormComponent implements OnInit {
  private fb = inject(FormBuilder)
  
  deckForm!: FormGroup;

  saveButtonName = input<string>('Save Deck');
  deck = input(null, {
    transform: (value: Deck) => {
      this.patchForm(value);
      return value;
    }
  });

  submit = output<Deck>();

  ngOnInit(): void {
    this.initForm();
  }

  onSubmit(): void {
    if (this.deckForm.valid) {
        const deck = this.formToDeck();
        this.submit.emit(deck);
        
        this.deckForm.reset();
      }
  }

  private initForm(): void {
    if (this.deckForm)
      return; // Form already initialized

    this.deckForm = this.fb.group({
        id: [''],
        name: ['', Validators.required],
        description: [''],
        maxReviews: [10, Validators.min(1)],
        maxNew: [5, Validators.min(1)]
    });
  }

  private patchForm(deck: Deck): void {
    if (deck) {
      this.initForm();

      this.deckForm.patchValue({
        id: deck.id,
        name: deck?.deckName,
        description: deck.description,
        maxReviews: deck.deckSettings?.reviewsPerDay,
        maxNew: deck.deckSettings?.newCardsPerDay
      });
    }
  }

  private formToDeck(): Deck {
      const deckForm = this.deckForm.value;

      return {
          id: deckForm.id,
          deckName: deckForm.name,
          description: deckForm.description,
          deckSettings: {
              reviewsPerDay: deckForm.maxReviews,
              newCardsPerDay: deckForm.maxNew
          }
      }
  }
}
