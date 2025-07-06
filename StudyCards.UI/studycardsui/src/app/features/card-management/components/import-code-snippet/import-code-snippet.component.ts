import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-import-code-snippet',
  imports: [],
  templateUrl: './import-code-snippet.component.html',
  styleUrl: './import-code-snippet.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ImportCodeSnippetComponent {
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
}
