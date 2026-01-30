import { ChangeDetectionStrategy, Component, effect, input, OnDestroy, output } from '@angular/core';
import { DeterminateProgressSpinnerComponent } from "../../../../shared/components/determinate-progress-spinner/determinate-progress-spinner.component";
import { MatIcon } from '@angular/material/icon';
import { MatMiniFabButton } from '@angular/material/button';
import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";
import { createAutoEvoke } from '../../../../shared/functions/auto-evoke';

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
  
  showSaveGroup = input(false);

  constructor() {
    effect(() => {
      if (this.showSaveGroup()) {
        this.autoSave.start();
      } else {
        this.autoSave.clear();
      }
    });
  }

  goBack = output<void>();
  save = output<void>();

  ngOnDestroy(): void {
    this.autoSave.clear();
  }
}
