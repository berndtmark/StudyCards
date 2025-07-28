import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardListPaginatorComponent } from './card-list-paginator.component';
import { CardStore } from '../../store/card.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('CardListPaginatorComponent', () => {
  let component: CardListPaginatorComponent;
  let fixture: ComponentFixture<CardListPaginatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardListPaginatorComponent],
      providers: [
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
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
