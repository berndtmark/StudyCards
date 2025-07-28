import { ChangeDetectionStrategy, Component } from '@angular/core';
import {PageEvent, MatPaginatorModule} from '@angular/material/paginator';

@Component({
  selector: 'app-card-list-paginator',
  imports: [MatPaginatorModule],
  templateUrl: './card-list-paginator.component.html',
  styleUrl: './card-list-paginator.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListPaginatorComponent {
  length = 50;
  pageSize = 10;
  pageIndex = 0;
  pageSizeOptions = [5, 10, 25];

  hidePageSize = false;
  showPageSizeOptions = true;
  showFirstLastButtons = true;
  disabled = false;

  //pageEvent?: PageEvent;

  handlePageEvent(e: PageEvent) {
    //this.pageEvent = e;
    this.length = e.length;
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;
  }
}
