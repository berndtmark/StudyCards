import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { CardStore } from 'app/features/card-management/store/card.store';
import { AddUpdateCardBaseComponent } from '../add-update-card-base.component';

@Component({
    selector: 'app-update-card-form',
    imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
    templateUrl: '../add-update-card-form.component.html',
    styleUrl: '../add-update-card-form.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpdateCardFormComponent extends AddUpdateCardBaseComponent implements OnInit {
    private route = inject(ActivatedRoute);
    readonly store = inject(CardStore);

    saveButtonName = "Update Card";
    private cardId?: string;

    constructor() {
        super();

        this.cardId = this.route.snapshot.paramMap.get('cardid') || '';
        effect(() => {
            this.patchFormValues(this.cardId!);
        });
    }

    ngOnInit(): void {
        this.initForm();
    }

    onSubmit(): void {
        if (this.cardForm.valid) {
            const form = this.cardForm.value;
            this.store.updateCard({ 
                cardId: this.cardId!,
                cardFront: form.front, 
                cardBack: form.back,
            });
            
            this.cardForm.reset();
            this.goBackToCardList();
        }
    }

    private patchFormValues(cardId: string): void {
        const card = this.store.getCardById(cardId);
        
        if (card) {
            const result = this.cardToForm(card);
            this.cardForm.patchValue(result);
        }
    }
}
