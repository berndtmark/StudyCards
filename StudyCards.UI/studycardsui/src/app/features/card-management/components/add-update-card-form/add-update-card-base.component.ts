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
        const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
        this.router.navigate(['/cards', deckId]);
    }

    protected cardToForm(card: Card) {
        return {
            front: card.cardFront,
            back: card.cardBack
        };
    }
}
