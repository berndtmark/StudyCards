import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Card } from 'app/@api/models/card';

export class AddUpdateCardBaseComponent {
    private router = inject(Router);
    private activatedRoute = inject(ActivatedRoute);
    private fb = inject(FormBuilder)
    cardForm!: FormGroup;

    protected initForm(): void {
        this.cardForm = this.fb.group({
            front: ['', [Validators.required]],
            back: ['', [Validators.required]]
        });
    }

    protected goBackToCardList(): void {
        this.router.navigate(['..'], { 
            relativeTo: this.activatedRoute 
        });
    }

    protected formToCard(): Card {
        const cardForm = this.cardForm.value;

        return {
            cardFront: cardForm.front,
            cardBack: cardForm.back
        }
    }

    protected cardToForm(card: any) {
        return {
            front: card.front,
            back: card.back
        };
    }
}
