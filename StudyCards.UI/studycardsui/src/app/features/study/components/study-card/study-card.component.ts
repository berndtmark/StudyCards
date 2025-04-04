import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-study-card',
  templateUrl: './study-card.component.html',
  styleUrls: ['./study-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StudyCardComponent {
  @Input() frontText: string = '';
  @Input() backText: string = '';
  
  isFlipped: boolean = false;

  flipCard(): void {
    this.isFlipped = !this.isFlipped;
  }
}
