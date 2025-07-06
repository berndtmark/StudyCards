import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-file-upload',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FileUploadComponent {
  fileSelected = output<Event>();
  accept = input<string>(".json");

  async onFileSelected(event: Event) {
    this.fileSelected.emit(event);

    // Reset the input so the same file can be selected again
    const input = event.target as HTMLInputElement;
    input.value = '';
  }
}
