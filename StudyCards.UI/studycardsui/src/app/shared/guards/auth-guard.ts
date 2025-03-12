import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { catchError, map, of } from 'rxjs';
import { HttpErrorResponse } from "@angular/common/http";
import { AuthorizeService } from "../services/authorize.service";

export const AuthGuard: CanActivateFn = () => {
    const authService = inject(AuthorizeService);
    const router = inject(Router);
    
    return authService.isLoggedIn().pipe(
        map(() => true),
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                router.navigate(['/login']);
            }
            return of(false);
        })
    );
};