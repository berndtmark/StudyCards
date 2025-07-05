import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { FileService } from 'app/shared/services/file.service';
import { ImportCard } from '../../models/import-card';
import { CardStore } from '../../store/card.store';
import { DialogService } from 'app/shared/services/dialog.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-card-import',
  imports: [MatButtonModule, JsonPipe],
  templateUrl: './card-import.component.html',
  styleUrl: './card-import.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardImportComponent {
  fileService = inject(FileService);
  dialog = inject(DialogService);
  readonly store = inject(CardStore);

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
    try {
      const data = await this.fileService.readJsonFile<ImportCard[]>(event);
      const isValid = ImportCard.isValid(data);
      if (!isValid)
        throw new Error();

      this.store.addImportCards(data);
    } catch (error) {
      this.dialog.info("Error Importing", "The File is not in a recognisable format or is badly formed");
    }
  }
}
