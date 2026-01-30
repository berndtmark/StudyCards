import { ChangeDetectionStrategy, Component, effect, input, linkedSignal, output, signal } from '@angular/core';
import { Deck } from '../../models/deck';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { form, FormField, min, required, validate, ValidationError } from '@angular/forms/signals'

@Component({
  selector: 'app-deck-form',
  imports: [FormField, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './deck-form.component.html',
  styleUrl: './deck-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeckFormComponent {  
  saveButtonName = input<string>('Save Deck');
  deck = input<Deck>();
  submit = output<Deck>();

  deckModel = linkedSignal({
    source: this.deck,
    computation: (deck) => {
      return {
        id: deck?.id ?? '',
        name: deck?.deckName ?? '',
        description: deck?.description ?? '',
        maxReviews: deck?.deckSettings?.reviewsPerDay ?? 10,
        maxNew: deck?.deckSettings?.newCardsPerDay ?? 2
      }
    }
  })

  deckForm = form(this.deckModel, (schemaPath) => {
    required(schemaPath.name, { message: 'Name is required' }),
    min(schemaPath.maxNew, 1, { message: 'Max New must be at least 1' }),
    min(schemaPath.maxReviews, 1, { message: 'Max New must be at least 1' }),
    validate(schemaPath.maxNew, ({value, valueOf}) => {
      const maxNew = value();
      const maxReviews = valueOf(schemaPath.maxReviews);

      if (maxNew != null && maxReviews != null && maxNew > maxReviews) {
        return { 
          kind: 'maxNewExceedsReviews',
          message: 'Max new cards must be less than or equal to max reviews'
        };
      }

      return null;
    })
  });

  onSubmit(): void {
    if (this.deckForm().valid()) {
        const deck = this.formToDeck();
        this.submit.emit(deck);
        
        this.deckForm().reset();
      }
  }

  hasMaxNewExceedsReviewsError(): boolean {
    return this.deckForm.maxNew().errors().some((error: ValidationError) => error.kind === 'maxNewExceedsReviews');
  }

  private formToDeck(): Deck {
      const deckForm = this.deckForm().value();

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
