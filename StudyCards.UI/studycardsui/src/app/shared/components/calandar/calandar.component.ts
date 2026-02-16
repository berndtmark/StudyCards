import { ChangeDetectionStrategy, Component, computed, input, signal, WritableSignal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { TruncateDirective } from '../../directives/truncate.directive';

const DAYS_PER_WEEK = 7;
const MONTH_NAMES = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
const DAY_NAMES = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

export interface CalendarItem {
  date: Date;
  label: string;
}

interface CalendarCell {
  displayName: string;
  day: number;
  isCurrentMonth: boolean;
}

@Component({
  selector: 'app-calandar',
  imports: [MatButtonModule, MatIconModule, MatChipsModule, TruncateDirective],
  templateUrl: './calandar.component.html',
  styleUrl: './calandar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Calandar {
  readonly items = input<CalendarItem[]>([]);
  readonly minDate = input<Date>();
  readonly maxDate = input<Date>();
 
  readonly viewMonth: WritableSignal<Date> = signal(new Date());
  readonly weekdays = DAY_NAMES;

  protected readonly monthYearLabel = computed(() => {
    const date = this.viewMonth();
    return `${MONTH_NAMES[date.getMonth()]} ${date.getFullYear()}`;
  });

  protected readonly canGoPrev = computed(() => {
    const min = this.minDate();
    if (!min) return true;
   
    const current = this.viewMonth();
    return current.getFullYear() > min.getFullYear() ||
           (current.getFullYear() === min.getFullYear() && current.getMonth() > min.getMonth());
  });

  protected readonly canGoNext = computed(() => {
    const max = this.maxDate();
    if (!max) return true;
   
    const current = this.viewMonth();
    return current.getFullYear() < max.getFullYear() ||
           (current.getFullYear() === max.getFullYear() && current.getMonth() < max.getMonth());
  });

  readonly calendar = computed(() => {
    const date = this.viewMonth();
    const year = date.getFullYear();
    const month = date.getMonth();
    const daysInMonth = new Date(year, month + 1, 0).getDate();
    const firstDayOfWeek = new Date(year, month, 1).getDay();

    const calendar: CalendarCell[][] = [[]];
    let currentWeek = 0;

    // Add empty cells for days before the first of the month
    for (let i = 0; i < firstDayOfWeek; i++) {
      const prevMonth = new Date(year, month, 0);
      const prevMonthDays = prevMonth.getDate();
      calendar[currentWeek].push({
        displayName: String(prevMonthDays - firstDayOfWeek + i + 1),
        day: prevMonthDays - firstDayOfWeek + i + 1,
        isCurrentMonth: false,
      });
    }

    // Add days of the current month
    for (let day = 1; day <= daysInMonth; day++) {
      if (calendar[currentWeek].length === DAYS_PER_WEEK) {
        currentWeek++;
        calendar.push([]);
      }
      calendar[currentWeek].push({
        displayName: String(day),
        day,
        isCurrentMonth: true,
      });
    }

    // Add empty cells for days after the last of the month
    const remainingCells = DAYS_PER_WEEK - calendar[currentWeek].length;
    for (let i = 1; i <= remainingCells; i++) {
      calendar[currentWeek].push({
        displayName: String(i),
        day: i,
        isCurrentMonth: false,
      });
    }

    return calendar;
  });

  nextMonth(): void {
    if (!this.canGoNext()) return;
   
    const current = this.viewMonth();
    this.viewMonth.set(new Date(current.getFullYear(), current.getMonth() + 1, 1));
  }

  prevMonth(): void {
    if (!this.canGoPrev()) return;
   
    const current = this.viewMonth();
    this.viewMonth.set(new Date(current.getFullYear(), current.getMonth() - 1, 1));
  }

  getItemsForDate(day: number, isCurrentMonth: boolean): CalendarItem[] {
    if (!isCurrentMonth) return [];
   
    const date = this.viewMonth();
    const targetDate = new Date(date.getFullYear(), date.getMonth(), day);
   
    return this.items().filter(item => {
      const itemDate = new Date(item.date);
      return itemDate.getFullYear() === targetDate.getFullYear() &&
             itemDate.getMonth() === targetDate.getMonth() &&
             itemDate.getDate() === targetDate.getDate();
    });
  }
}
