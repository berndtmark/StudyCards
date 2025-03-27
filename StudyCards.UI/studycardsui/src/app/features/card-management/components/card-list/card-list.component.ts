import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { Card } from 'app/@api/models/card';
import { CardStore } from '../../store/card.store';
import { ActivatedRoute } from '@angular/router';
import { MatProgressBar } from '@angular/material/progress-bar';
import { NgIf } from '@angular/common';
import { LoadingState } from 'app/shared/models/loading-state';

@Component({
  selector: 'app-card-list',
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatProgressBar, NgIf],
  templateUrl: './card-list.component.html',
  styleUrl: './card-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CardListComponent implements OnInit {
  readonly store = inject(CardStore);
  activatedRoute = inject(ActivatedRoute)

  loadingState = LoadingState;
  displayedColumns: string[] = ['cardfront', 'cardback'];
  dataSource = new MatTableDataSource<Card>([]);

  constructor() {
    effect(() => {
      this.dataSource.data = this.store.cards();
    });
  }

  ngOnInit(): void {
    const deckId = this.activatedRoute.snapshot.paramMap.get('deckid');
    this.store.loadCards(deckId!);
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
