import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { CardResponse } from 'app/@api/models/card-response';

@Component({
  selector: 'app-card-list',
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatIconModule],
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

  displayedColumns: string[] = ['cardfront', 'cardback'];
  dataSource = new MatTableDataSource<CardResponse>([]);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
