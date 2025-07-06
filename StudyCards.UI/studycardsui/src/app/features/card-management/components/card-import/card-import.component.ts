import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CardStore } from '../../store/card.store';
import { CardImportDisplayComponent } from "../card-import-display/card-import-display.component";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-card-import',
  imports: [MatButtonModule, CardImportDisplayComponent],
  templateUrl: './card-import.component.html',
  styleUrl: './card-import.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardImportComponent implements OnInit {
  readonly store = inject(CardStore);
  activatedRoute = inject(ActivatedRoute);

  selectedItemsForImport = signal<string[]>([]);
  isImportStarted = computed(() => this.store.importCards().file);

  codeSnippet = `[
    {
      "cardFront": "What is the capital of France",
      "cardBack": "Paris"
    },
    {
      "cardFront": "What is 2 + 2",
      "cardBack": "4"
    }
]`;

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    this.store.loadDeckIfNot(deckId!);
  }

  async onFileSelected(event: Event) {
    this.store.importCardsFromFile(event);
  }

  import() {
    this.store.import({ cardsIdsToImport: this.selectedItemsForImport() });
  }

  updateSelectedItems(ids: string[]) {
    this.selectedItemsForImport.set([...ids]);
  }
}
