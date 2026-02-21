import { ChangeDetectionStrategy, Component, inject, output } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NavMenuService } from '../../../services/nav-menu.service';
import { MatIconModule } from "@angular/material/icon";
import { MyButtonComponent } from "../../my-button/my-button.component";

@Component({
  selector: 'app-side-nav',
  imports: [MatListModule, RouterLink, RouterLinkActive, MatIconModule, MyButtonComponent],
  templateUrl: './side-nav.component.html',
  styleUrl: './side-nav.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavComponent {
  protected readonly navMenu = inject(NavMenuService);
  menuSelected = output();
}
