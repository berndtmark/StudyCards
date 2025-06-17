import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-study-complete',
  templateUrl: './study-complete.component.html',
  styleUrls: ['./study-complete.component.scss'],
  standalone: true,
  imports: [CommonModule, MatIconModule]
})
export class StudyCompleteComponent {
    title = input<string>('All Done for Today!');
    subtitle = input<string>('Great job! You\'ve completed all your study cards for today.');
}
