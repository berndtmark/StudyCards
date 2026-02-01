import { ChangeDetectionStrategy, Component, effect, input, linkedSignal, OnInit, output, signal } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CardResponse } from '../../../../@api/models/card-response';
import { form, required, FormField } from '@angular/forms/signals';

@Component({
  selector: 'app-card-form',
  imports: [MatInputModule, MatButtonModule, FormField],
  templateUrl: './card-form.component.html',
  styleUrl: './card-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardFormComponent {
  title = input<string>();
  saveButtonName = input<string>('Save Card');
  card = input<CardResponse>();
  submitted = output<{cardFront: string, cardBack: string}>();

  cardModel = linkedSignal({
    source: this.card,
    computation: (card) => {
      return {
        front: card?.cardFront ?? '',
        back: card?.cardBack ?? ''
      }
    }
  });

  cardForm = form(this.cardModel, (schemaPath) => {
    required(schemaPath.front, { message: 'Front is required' }),
    required(schemaPath.back, { message: 'Back is required' })
  });

  onSubmit(): void {
    if (this.cardForm().valid()) {
        const form = this.cardForm().value();
        this.submitted.emit({
          cardFront: form.front,
          cardBack: form.back
        });
      }
  }
}
