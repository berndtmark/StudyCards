import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { DeterminateProgressSpinnerComponent } from './determinate-progress-spinner.component';

describe('DeterminateProgressSpinnerComponent', () => {
  let component: DeterminateProgressSpinnerComponent;
  let fixture: ComponentFixture<DeterminateProgressSpinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeterminateProgressSpinnerComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeterminateProgressSpinnerComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
