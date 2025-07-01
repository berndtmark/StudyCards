import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { CardResponse } from 'app/@api/models/card-response';
import {MatSlideToggleChange, MatSlideToggleModule} from '@angular/material/slide-toggle';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-card-list',
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatIconModule, MatSlideToggleModule, DatePipe],
  templateUrl: './card-list.component.html',
  styleUrl: './card-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListComponent {
  cards = input([], {
    transform: (value: CardResponse[]) => {
      this.dataSource.data = value;
      return value;
    }
  });
  updateCard = output<string>();

  private defaultColumns = ['cardfront', 'cardback'];
  private extendedColumns =  ['cardfront', 'cardback', 'nextstudy', 'studycount'];
  displayedColumns: string[] = this.defaultColumns;
  dataSource = new MatTableDataSource<CardResponse>([]);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  showMoreToggle(change: MatSlideToggleChange) {
    this.displayedColumns = change.checked ? this.extendedColumns : this.defaultColumns;
  }
}
