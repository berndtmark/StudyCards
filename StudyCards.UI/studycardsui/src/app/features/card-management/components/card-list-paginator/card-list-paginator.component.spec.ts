import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardListPaginatorComponent } from './card-list-paginator.component';
import { CardStore } from '../../store/card.store';

describe('CardListPaginatorComponent', () => {
  let component: CardListPaginatorComponent;
  let fixture: ComponentFixture<CardListPaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardListPaginatorComponent],
      providers: [
        CardStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardListPaginatorComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
