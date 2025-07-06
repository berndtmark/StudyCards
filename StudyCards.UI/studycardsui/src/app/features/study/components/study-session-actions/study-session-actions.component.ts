import { ChangeDetectionStrategy, Component, input, OnDestroy, output } from '@angular/core';
import { DeterminateProgressSpinnerComponent } from "../../../../shared/components/determinate-progress-spinner/determinate-progress-spinner.component";
import { createAutoEvoke } from 'app/shared/functions/auto-evoke';
import { MatIcon } from '@angular/material/icon';
import { MatMiniFabButton } from '@angular/material/button';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";

@Component({
  selector: 'app-study-session-actions',
  imports: [DeterminateProgressSpinnerComponent, MatIcon, MatMiniFabButton, MyButtonComponent],
  templateUrl: './study-session-actions.component.html',
  styleUrl: './study-session-actions.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionActionsComponent implements OnDestroy {
  autoSaveTimeSeconds = 120;
  autoSave = createAutoEvoke(() => this.save.emit(), this.autoSaveTimeSeconds);
  
  showSaveGroup = input(false, {
    transform: (value: boolean) => {
      if (value) {
        this.autoSave.start();
      } else {
        this.autoSave.clear();
      }

      return value;
    }
  });

  goBack = output<void>();
  save = output<void>();

  ngOnDestroy(): void {
    this.autoSave.clear();
  }
}
