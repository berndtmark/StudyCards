import { inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Card } from 'app/@api/models/card';
import { CardStore } from '../../store/card.store';
import { DialogService } from 'app/shared/services/dialog.service';

export abstract class AddUpdateCardBaseComponent {
    private router = inject(Router);
    protected activatedRoute = inject(ActivatedRoute);
    private fb = inject(FormBuilder);
    private dialogService = inject(DialogService);
    readonly store = inject(CardStore);
    
    cardForm!: FormGroup;

    protected cardId?: string;

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

    removeCard(): void {
        if (!this.cardId) 
            throw new Error("Card ID is not defined");

        this.dialogService.confirm('Delete Card', 'Are you sure you want to remove this Card?', 'Yes')
            .subscribe(() => {
                this.store.removeCard(this.cardId!);
                this.goBackToCardList();
            });
    }
}
