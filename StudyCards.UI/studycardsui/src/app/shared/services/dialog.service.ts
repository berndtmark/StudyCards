import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { GenericDialogComponent } from '../components/generic-dialog/generic-dialog.component';
import { filter, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  dialog = inject(MatDialog);

  info(title: string, message: string) {
    this.dialog.open(GenericDialogComponent, {
      data: {
        title,
        message,
        okButtonText: 'OK'
      } as DialogData,
    });
  }

  confirm(title: string, message: string, okButtonText: string): Observable<boolean> {
    const dialogRef = this.dialog.open(GenericDialogComponent, {
      data: {
        title,
        message,
        okButtonText,
        cancelButtonText: 'Cancel'
      } as DialogData,
    });

    return dialogRef.afterClosed().pipe(filter(r => !!r));
  }
}

export interface DialogData {
  title: string;
  message: string;
  okButtonText?: string;
  cancelButtonText?: string;
}