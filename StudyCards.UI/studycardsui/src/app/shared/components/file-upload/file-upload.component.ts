import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MyButtonComponent } from "../my-button/my-button.component";

@Component({
  selector: 'app-file-upload',
  imports: [MyButtonComponent],
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
