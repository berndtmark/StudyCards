import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { CardStore } from '../../store/card.store';
import { CardImportDisplayComponent } from "../card-import-display/card-import-display.component";
import { ActivatedRoute, Router } from '@angular/router';
import { ImportCodeSnippetComponent } from "../import-code-snippet/import-code-snippet.component";
import { MatProgressBar } from '@angular/material/progress-bar';
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";
import { FileUploadComponent } from "../../../../shared/components/file-upload/file-upload.component";

@Component({
  selector: 'app-card-import',
  imports: [MatButtonModule, CardImportDisplayComponent, ImportCodeSnippetComponent, MatProgressBar, BackNavComponent, FileUploadComponent],
  templateUrl: './card-import.component.html',
  styleUrl: './card-import.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardImportComponent implements OnInit {
  readonly store = inject(CardStore);
  activatedRoute = inject(ActivatedRoute);
  router = inject(Router);

  selectedItemsForImport = signal<string[]>([]);
  deckId: string | null = '';

  ngOnInit(): void {
    this.deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    this.store.loadDeckIfNot(this.deckId!);
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

  goBack(): void {
    this.router.navigate(['/cards', this.deckId]);
  }
}
