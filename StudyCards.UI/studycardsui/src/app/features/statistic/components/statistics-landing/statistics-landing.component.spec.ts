import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticsLandingComponent } from './statistics-landing.component';
import { StatisticStore } from '../../store/statistic.store';

describe('StatisticsLandingComponent', () => {
  let component: StatisticsLandingComponent;
  let fixture: ComponentFixture<StatisticsLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatisticsLandingComponent],
      providers: [StatisticStore]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StatisticsLandingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
