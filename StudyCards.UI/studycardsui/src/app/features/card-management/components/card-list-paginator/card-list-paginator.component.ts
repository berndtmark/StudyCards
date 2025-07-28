import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {PageEvent, MatPaginatorModule} from '@angular/material/paginator';
import { CardStore } from '../../store/card.store';

@Component({
  selector: 'app-card-list-paginator',
  imports: [MatPaginatorModule],
  templateUrl: './card-list-paginator.component.html',
  styleUrl: './card-list-paginator.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListPaginatorComponent {
  readonly store = inject(CardStore);

  pageSizeOptions = [10, 25, 50];

  handlePageEvent(e: PageEvent) {
    this.store.paginate(e.pageIndex + 1, e.pageSize)
  }
}
