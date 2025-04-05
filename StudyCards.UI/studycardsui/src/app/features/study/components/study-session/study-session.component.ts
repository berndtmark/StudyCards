import { ChangeDetectionStrategy, Component } from '@angular/core';
import { StudyCardComponent } from '../study-card/study-card.component';

@Component({
  selector: 'app-study-session',
  imports: [StudyCardComponent],
  templateUrl: './study-session.component.html',
  styleUrl: './study-session.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudySessionComponent {

}
