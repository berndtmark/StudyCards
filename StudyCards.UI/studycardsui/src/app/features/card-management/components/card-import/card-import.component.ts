import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CardStore } from '../../store/card.store';
import { CardImportDisplayComponent } from "../card-import-display/card-import-display.component";

@Component({
  selector: 'app-card-import',
  imports: [MatButtonModule, CardImportDisplayComponent],
  templateUrl: './card-import.component.html',
  styleUrl: './card-import.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardImportComponent {
  readonly store = inject(CardStore);

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

  async onFileSelected(event: Event) {
    this.store.importCardsFromFile(event);
  }

  import() {
    console.log(this.selectedItemsForImport());
    this.store.import();
  }

  updateSelectedItems(ids: string[]) {
    this.selectedItemsForImport.set([...ids]);
  }
}
