
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogModule } from '@angular/material/dialog';
import { DialogData } from '../../services/dialog.service';

@Component({
  selector: 'app-generic-dialog',
  imports: [MatDialogTitle, MatDialogContent, MatDialogActions, MatDialogModule, MatButtonModule],
  templateUrl: './generic-dialog.component.html',
  styleUrl: './generic-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GenericDialogComponent {
  data: DialogData = inject(MAT_DIALOG_DATA);
}
