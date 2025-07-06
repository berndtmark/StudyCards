import { ChangeDetectionStrategy, Component, input, OnInit, output } from '@angular/core';
import { ImportCard } from '../../models/import-card';
import { MatTableModule } from '@angular/material/table';
import { SelectionModel } from '@angular/cdk/collections';
import {MatCheckboxModule} from '@angular/material/checkbox';

@Component({
  selector: 'app-card-import-display',
  imports: [MatTableModule, MatCheckboxModule],
  templateUrl: './card-import-display.component.html',
  styleUrl: './card-import-display.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardImportDisplayComponent implements OnInit {
  cards = input([], {
    transform: (value: ImportCard[]) => {
      this.selection.clear();
      this.updateEmitter();
      return value;
    }
  });
  title = input<string>();
  includeSelector = input<boolean>();
  selectedIds = output<string[]>();

  displayedColumns = ['cardfront', 'cardback'];
  selection = new SelectionModel<ImportCard>(true, []);

  ngOnInit() {
    if (this.includeSelector()) {
      this.displayedColumns.unshift('select');
    }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.cards().length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
    } else {
      this.selection.select(...this.cards());
    }

    this.updateEmitter();
  }

  toggle(row: ImportCard) {
    this.selection.toggle(row);
    this.updateEmitter();
  }

  private updateEmitter() {
    this.selectedIds.emit(this.selection.selected.map(s => s.id!));
  }
}
