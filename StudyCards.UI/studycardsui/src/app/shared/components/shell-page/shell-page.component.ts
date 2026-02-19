
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterOutlet } from '@angular/router';
import { AuthorizeService } from '../../services/authorize.service';
import { RootStore } from '../../store/root.store';
import {MatSidenavModule} from '@angular/material/sidenav';
import { SideNavComponent } from './side-nav/side-nav.component';
import {MatRippleModule} from '@angular/material/core';

@Component({
  selector: 'app-shell-page',
  imports: [RouterOutlet, MatButtonModule, MatIcon, MatSidenavModule, SideNavComponent, MatRippleModule],
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
