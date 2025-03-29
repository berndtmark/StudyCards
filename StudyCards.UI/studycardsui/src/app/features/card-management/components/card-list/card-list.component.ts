import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { Card } from 'app/@api/models/card';

@Component({
  selector: 'app-card-list',
  imports: [MatFormFieldModule, MatInputModule, MatTableModule],
  templateUrl: './card-list.component.html',
  styleUrl: './card-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListComponent {
  @Input() set cards(value: Card[]) {
    this.dataSource.data = value;
  }
  @Output() updateCard = new EventEmitter<string>();

  displayedColumns: string[] = ['cardfront', 'cardback'];
  dataSource = new MatTableDataSource<Card>([]);

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
