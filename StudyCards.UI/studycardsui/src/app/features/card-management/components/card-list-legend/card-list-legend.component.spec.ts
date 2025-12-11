import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardListLegendComponent } from './card-list-legend.component';

describe('CardListLegendComponent', () => {
  let component: CardListLegendComponent;
  let fixture: ComponentFixture<CardListLegendComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardListLegendComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardListLegendComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
