import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { AuthService } from "../../@api/services";
import { catchError, map, of } from 'rxjs';
import { HttpErrorResponse } from "@angular/common/http";

export const AuthGuard: CanActivateFn = () => {
    const authService = inject(AuthService);
    const router = inject(Router);
    
    return authService.apiAuthIsloggedinGet().pipe(
        map(() => true),
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                router.navigate(['/login']);
            }
            return of(false);
        })
    );
};