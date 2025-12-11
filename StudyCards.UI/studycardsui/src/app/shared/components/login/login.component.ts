import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import { AuthorizeService } from '../../services/authorize.service';

@Component({
  selector: 'app-login',
  imports: [MatButtonModule, MatIcon],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent {
  private authService = inject(AuthorizeService);

  signInWithGoogle() {
    this.authService.login();
  }
}
