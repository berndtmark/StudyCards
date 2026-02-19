import { ChangeDetectionStrategy, Component, inject, output } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NavMenuService } from '../../../services/nav-menu.service';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-side-nav',
  imports: [MatListModule, RouterLink, RouterLinkActive, MatIcon],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavComponent {
  protected readonly navMenu = inject(NavMenuService);
  menuSelected = output();
}
