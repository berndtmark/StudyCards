import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-determinate-progress-spinner',
  imports: [MatProgressSpinnerModule],
  templateUrl: './determinate-progress-spinner.component.html',
  styleUrl: './determinate-progress-spinner.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DeterminateProgressSpinnerComponent {
  size = input<number>(25);
  progress = input<number>(0);
}
