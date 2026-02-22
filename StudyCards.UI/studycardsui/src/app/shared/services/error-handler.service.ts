import { inject, Injectable } from '@angular/core';
import { DialogService } from './dialog.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackbarService } from './snackbar.service';
import { LoadingState } from '../models/loading-state';
import { of } from 'rxjs';
import { patchState } from '@ngrx/signals';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {
  private dialogService = inject(DialogService);
  private snackBar = inject(SnackbarService);

  // legacy - tapResponse if the newer method
  handleRxJSStoreError(store: any, errorMessage: string) {
    return (err: any) => {
      this.handleTapError(store, errorMessage, err);
      return of(null);
    };
  }

  handleTapError(store: any, errorMessage: string, err: HttpErrorResponse | any): void {
    if (!this.handleValidationError(err)) {
      // Not a validation error: Show snackbar and set store to Error
      patchState(store, { loadingState: LoadingState.Error });
      this.snackBar.open(errorMessage);
    } else {
      // It's a validation result, let the app continue as normal
      patchState(store, { loadingState: LoadingState.Success }); 
    }
  }

  private handleValidationError(err: HttpErrorResponse): boolean {
      if ([400, 404, 409].includes(err.status)) {
          this.dialogService.info('Validation', err?.error?.detail ?? "Unknown Error has occurred");
          return true;
      }

      return false;
  };
}