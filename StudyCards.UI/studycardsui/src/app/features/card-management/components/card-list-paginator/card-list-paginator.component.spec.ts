import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardListPaginatorComponent } from './card-list-paginator.component';

describe('CardListPaginatorComponent', () => {
  let component: CardListPaginatorComponent;
  let fixture: ComponentFixture<CardListPaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardListPaginatorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardListPaginatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
