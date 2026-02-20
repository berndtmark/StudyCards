import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-top-header',
  imports: [MatIcon, MatRippleModule, MatButtonModule, MatIcon],
  templateUrl: './top-header.component.html',
  styleUrl: './top-header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopHeaderComponent {
  userName = input<string>();
  logout = output<void>();
  menuSelected = output<void>();
}
