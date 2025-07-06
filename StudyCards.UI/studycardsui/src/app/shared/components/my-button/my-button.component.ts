import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-my-button',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './my-button.component.html',
  styleUrl: './my-button.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyButtonComponent {
  title = input<string>();
  icon = input<string>();
  disabled = input<boolean>();
  buttonType = input<'elevated' | 'filled' | 'tonal' | 'outlined'>("elevated");

  action = output<void>();
}
