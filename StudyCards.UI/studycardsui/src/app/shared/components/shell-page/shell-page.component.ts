
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { AuthorizeService } from '../../services/authorize.service';
import { RootStore } from '../../store/root.store';

@Component({
  selector: 'app-shell-page',
  imports: [RouterOutlet, MatButtonModule, MatIcon],
  templateUrl: './shell-page.component.html',
  styleUrl: './shell-page.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ShellPageComponent {
  private authService = inject(AuthorizeService);
  readonly store = inject(RootStore);

  logout() {
    this.authService.logout();
  }
}
