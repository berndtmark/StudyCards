import { ChangeDetectionStrategy, Component } from '@angular/core';
import { StudyCardComponent } from '../study-card/study-card.component';

@Component({
  selector: 'app-study-landing',
  imports: [StudyCardComponent],
  templateUrl: './study-landing.component.html',
  styleUrl: './study-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyLandingComponent {

}
