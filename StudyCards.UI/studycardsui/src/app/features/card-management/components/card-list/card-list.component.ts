import { ChangeDetectionStrategy, Component, input, OnChanges, output, SimpleChanges } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import {MatSlideToggleChange, MatSlideToggleModule} from '@angular/material/slide-toggle';
import { DatePipe } from '@angular/common';
import { CardListPaginatorComponent } from "../card-list-paginator/card-list-paginator.component";
import { CardListLegendComponent } from "../card-list-legend/card-list-legend.component";
import { CardResponse } from '../../../../@api/models/card-response';

@Component({
  selector: 'app-card-list',
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatIconModule, MatSlideToggleModule, DatePipe, CardListPaginatorComponent, CardListLegendComponent],
  templateUrl: './card-list.component.html',
  styleUrl: './card-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListComponent implements OnChanges {
  searchTerm = input<string>();
  cards = input([], {
    transform: (value: CardResponse[]) => {
      this.dataSource.data = value;
      return value;
    }
  });
  updateCard = output<string>();
  search = output<string>();

  private defaultColumns = ['cardfront', 'cardback'];
  private extendedColumns =  ['reviewphase', 'cardfront', 'cardback', 'nextstudy', 'studycount'];
  displayedColumns: string[] = this.defaultColumns;
  dataSource = new MatTableDataSource<CardResponse>([]);
  searchValue: string = '';

  ngOnChanges(changes: SimpleChanges): void {
    const seachTermChanges = changes['searchTerm'];
    
    if (seachTermChanges && seachTermChanges.isFirstChange()) {
      this.searchValue = this.searchTerm() ?? '';
    } else if (seachTermChanges && seachTermChanges.currentValue === undefined) {
      this.searchValue = '';
    }    
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    const searchTerm = filterValue.trim().toLowerCase();
    this.search.emit(searchTerm);
  }

  showMoreToggle(change: MatSlideToggleChange) {
    this.displayedColumns = change.checked ? this.extendedColumns : this.defaultColumns;
  }
}
