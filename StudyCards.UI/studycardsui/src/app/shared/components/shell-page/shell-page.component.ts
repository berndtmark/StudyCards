
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthorizeService } from '../../services/authorize.service';
import { RootStore } from '../../store/root.store';
import {MatSidenavModule} from '@angular/material/sidenav';
import { SideNavComponent } from './side-nav/side-nav.component';
import { TopHeaderComponent } from "./top-header/top-header.component";

@Component({
  selector: 'app-shell-page',
  imports: [RouterOutlet, MatSidenavModule, SideNavComponent, TopHeaderComponent],
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
