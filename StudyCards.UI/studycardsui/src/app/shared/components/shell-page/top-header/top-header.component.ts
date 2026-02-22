import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MyButtonComponent } from "../../my-button/my-button.component";

@Component({
  selector: 'app-top-header',
  imports: [MyButtonComponent],
  templateUrl: './top-header.component.html',
  styleUrl: './top-header.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TopHeaderComponent {
  userName = input<string>();
  logout = output<void>();
  menuSelected = output<void>();
}
