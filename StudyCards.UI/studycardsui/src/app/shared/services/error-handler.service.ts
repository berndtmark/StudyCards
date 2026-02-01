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

  handleStoreError(store: any, action: string) {
    return (err: any) => {
      if (!this.handleValidationError(err)) {
        patchState(store, { loadingState: LoadingState.Error });
        this.snackBar.open(action);
      } else {
        patchState(store, { loadingState: LoadingState.Success }); // Its a validation result, let the app continue as normal
      }

      return of(null);
    };
  }

  private handleValidationError(err: HttpErrorResponse): boolean {
      if ([400, 404, 409].includes(err.status)) {
          this.dialogService.info('Validation', err?.error?.detail ?? "Unknown Error has occurred");
          return true;
      }

      return false;
  };
}