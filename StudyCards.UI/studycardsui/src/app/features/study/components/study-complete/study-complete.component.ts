import { Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

import { MyButtonComponent } from "../../../../shared/components/my-button/my-button.component";

@Component({
  selector: 'app-study-complete',
  templateUrl: './study-complete.component.html',
  styleUrls: ['./study-complete.component.scss'],
  standalone: true,
  imports: [MatIconModule, MyButtonComponent]
})
export class StudyCompleteComponent {
    title = input<string>('All Done for Today!');
    subtitle = input<string>('Great job! You\'ve completed all your study cards for today.');
    done = output<void>();
}
