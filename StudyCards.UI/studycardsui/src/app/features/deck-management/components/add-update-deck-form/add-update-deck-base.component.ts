import { inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { DeckResponse } from "app/@api/models/deck-response";

export class AddUpdateDeckBaseComponent {
    private router = inject(Router);
    private fb = inject(FormBuilder)
    deckForm!: FormGroup;

    protected initForm() {
        this.deckForm = this.fb.group({
            id: [''],
            name: ['', Validators.required],
            description: [''],
            maxReviews: [10, Validators.min(1)],
            maxNew: [5, Validators.min(1)]
        });
    }

    goBackToDeckList(): void {
        this.router.navigate(['/decks']);
    }

    protected formToDeck(): DeckResponse {
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

    protected deckToForm(deck: DeckResponse) {
        return {
            id: deck.id,
            name: deck?.deckName,
            description: deck.description,
            maxReviews: deck.deckSettings?.reviewsPerDay,
            maxNew: deck.deckSettings?.newCardsPerDay
        }
    }
}