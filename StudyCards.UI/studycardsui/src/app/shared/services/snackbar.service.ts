import { inject, Service } from '@angular/core';
import {MatSnackBar} from '@angular/material/snack-bar';

@Service()
export class SnackbarService {
  private _snackBar = inject(MatSnackBar);

  open(message: string) {
    this._snackBar.open(message, '', { duration: 1000 });
  }
}
