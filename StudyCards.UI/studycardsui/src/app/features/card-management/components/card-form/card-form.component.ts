import { ChangeDetectionStrategy, Component, inject, input, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CardResponse } from 'app/@api/models/card-response';

@Component({
  selector: 'app-card-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './card-form.component.html',
  styleUrl: './card-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  
  cardForm!: FormGroup;

  title = input<string>();
  saveButtonName = input<string>('Save Card');
  card = input(null, {
    transform: (value: CardResponse) => {
      this.patchForm(value);
      return value;
    }
  });

  submitted = output<{cardFront: string, cardBack: string}>();
  
  ngOnInit(): void {
    this.initForm();
  }

  onSubmit(): void {
    if (this.cardForm.valid) {
        const form = this.cardForm.value;
        this.submitted.emit({
          cardFront: form.front,
          cardBack: form.back
        });
      }
  }

  private initForm(): void {
    if (this.cardForm)
      return; // Form already initialized

    this.cardForm = this.fb.group({
      front: ['', [Validators.required]],
      back: ['', [Validators.required]]
    });
  }

  private patchForm(card: CardResponse): void {
    if (card) {
      this.initForm();

      this.cardForm.patchValue({
        front: card.cardFront,
        back: card.cardBack
      });
    }
  }
}
