import { ChangeDetectionStrategy, Component, computed, inject, OnInit, signal } from '@angular/core';
import { Calandar, CalendarItem } from "../../../../shared/components/calandar/calandar.component";
import { StatisticStore } from '../../store/statistic.store';
import { DateFuctions } from '../../../../shared/functions/date-functions';
import { MatProgressBar } from "@angular/material/progress-bar";
import { LoadingState } from '../../../../shared/models/loading-state';

@Component({
  selector: 'app-statistics-landing',
  imports: [Calandar, MatProgressBar],
  templateUrl: './statistics-landing.component.html',
  styleUrl: './statistics-landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatisticsLandingComponent implements OnInit {
  readonly store = inject(StatisticStore);

  ngOnInit(): void {
    this.store.loadStudyStatisticsForMonth({ date: DateFuctions.today() });
  }
  
  protected readonly maxDate = signal(DateFuctions.today());
  protected readonly minDate = signal(DateFuctions.sinceInception());
  loadingState = LoadingState;

  calendarItems = computed<CalendarItem[]>(() => {
      return this.store.studyStatistics().map(s => ({
          date: s.dateRecorded ? new Date(s.dateRecorded) : new Date(),
          label: `${s.name}: ${s.cardsStudied}`,
          colour: "#f0f4c3"
      }));
  });

  calandarChange(date: Date): void {
    this.store.loadStudyStatisticsForMonth({ date });
  }
}
