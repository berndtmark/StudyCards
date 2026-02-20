import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { Calandar, CalendarItem } from "../../../../shared/components/calandar/calandar.component";
import { StatisticStore } from '../../store/statistic.store';
import { DateFuctions } from '../../../../shared/functions/date-functions';

@Component({
  selector: 'app-statistics-landing',
  imports: [Calandar],
  templateUrl: './statistics-landing.component.html',
  styleUrl: './statistics-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatisticsLandingComponent {
  readonly store = inject(StatisticStore);
  
  protected readonly maxDate = signal(DateFuctions.today());
  protected readonly minDate = signal(DateFuctions.oneYearAgo());

  calendarItems = computed<CalendarItem[]>(() => {
      return this.store.studyStatistics().map(s => ({
          date: s.dateRecorded ? new Date(s.dateRecorded) : new Date(),
          label: `${s.name}: ${s.cardsStudied}` 
      }));
  });
}
