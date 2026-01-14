import { inject, Injectable, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { map, Observable, Subject, takeUntil, tap } from 'rxjs';
import { AuthService } from '../../@api/services';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService implements OnDestroy {
  private authServiceApi = inject(AuthService);
  private router = inject(Router);
  
  private logInPath = '/api/auth/login';
  private $unsubscribe = new Subject<void>();

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }

  isLoggedIn(): Observable<void> {
    return this.authServiceApi.apiAuthIsloggedinGet();
  }

  login() {
    window.location.href = this.logInPath;
  }

  logout() {
    this.authServiceApi.apiAuthLogoutPost()
      .pipe(
        takeUntil(this.$unsubscribe),
        tap(() => this.router.navigate(['/login']))
      )
      .subscribe();
  }

  claims(): Observable<Map<string, string>> {
    return this.authServiceApi.apiAuthMeGet$Json()
        .pipe(
            map(claim => new Map(Object.entries(claim)))
        );
  }
}
