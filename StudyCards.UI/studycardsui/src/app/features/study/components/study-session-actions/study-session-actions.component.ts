import { ChangeDetectionStrategy, Component, input, OnDestroy, output } from '@angular/core';
import { BackNavComponent } from "../../../../shared/components/back-nav/back-nav.component";
import { DeterminateProgressSpinnerComponent } from "../../../../shared/components/determinate-progress-spinner/determinate-progress-spinner.component";
import { createAutoEvoke } from 'app/shared/functions/auto-evoke';
import { MatIcon } from '@angular/material/icon';
import { MatMiniFabButton } from '@angular/material/button';

@Component({
  selector: 'app-study-session-actions',
  imports: [BackNavComponent, DeterminateProgressSpinnerComponent, MatIcon, MatMiniFabButton],
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
