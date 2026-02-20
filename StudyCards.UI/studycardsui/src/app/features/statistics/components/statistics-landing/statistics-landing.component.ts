import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { Calandar, CalendarItem } from "../../../../shared/components/calandar/calandar.component";

@Component({
  selector: 'app-statistics-landing',
  imports: [Calandar],
  templateUrl: './statistics-landing.component.html',
  styleUrl: './statistics-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatisticsLandingComponent {
  protected readonly maxDate = signal(new Date());

  protected readonly sampleItems: CalendarItem[] = [
    { date: new Date(2026, 1, 14), label: 'Valentine\'s Day' },
    { date: new Date(2026, 1, 16), label: 'Meeting' },
    { date: new Date(2026, 1, 16), label: 'Doctor Appt' },
    { date: new Date(2026, 1, 20), label: 'Flags: 14' },
    { date: new Date(2026, 1, 20), label: 'Countries: 10' },
  ];
}
