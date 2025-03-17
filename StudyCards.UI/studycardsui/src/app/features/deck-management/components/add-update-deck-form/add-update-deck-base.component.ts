import { inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Deck } from "app/@api/models/deck";

export class AddUpdateDeckBaseComponent {
    private router = inject(Router);
    private fb = inject(FormBuilder)
    deckForm: FormGroup;

    constructor() {
        this.deckForm = this.fb.group({
            name: ['', Validators.required],
            description: [''],
            maxReviews: [10, Validators.min(1)],
            maxNew: [5, Validators.min(1)]
        });
    }

    goBackToDeckList(): void {
        this.router.navigate(['/decks']);
    }

    protected formToDeck(): Deck {
        const deckForm = this.deckForm.value;

        return {
          deckName: deckForm.name,
          description: deckForm.description,
          deckSettings: {
            reviewsPerDay: deckForm.maxReviews,
            newCardsPerDay: deckForm.newCardsPerDay
          }
        }
    }

    protected deckToForm(deck: Deck) {
        return {
            name: deck?.deckName,
            description: deck.description,
            maxReviews: deck.deckSettings?.reviewsPerDay,
            maxNew: deck.deckSettings?.newCardsPerDay
        }
    }
}