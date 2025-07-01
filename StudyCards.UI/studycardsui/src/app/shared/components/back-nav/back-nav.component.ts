import { ChangeDetectionStrategy, Component, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-back-nav',
  imports: [MatIconModule, MatButtonModule],
  templateUrl: './back-nav.component.html',
  styleUrl: './back-nav.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BackNavComponent {
  back = output<void>();
}
