import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
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
  noResponsive = input<boolean>();
  buttonType = input<'elevated' | 'filled' | 'tonal' | 'outlined' | 'icon'>("elevated");

  action = output<void>();

  responsiveIcon = computed(() => this.icon() && !this.noResponsive());

  matButtonType = computed(() => {
    const type = this.buttonType();
    return type === 'icon' ? 'elevated' : type; // Default to 'elevated' if it's an icon. Allow compiler to be happy as icon is not a [matButton] type
  });
}
