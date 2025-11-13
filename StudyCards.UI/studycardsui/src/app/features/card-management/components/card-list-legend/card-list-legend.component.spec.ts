import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { CardListLegendComponent } from './card-list-legend.component';

describe('CardListLegendComponent', () => {
  let component: CardListLegendComponent;
  let fixture: ComponentFixture<CardListLegendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardListLegendComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardListLegendComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
