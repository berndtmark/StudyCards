import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeterminateProgressSpinnerComponent } from './determinate-progress-spinner.component';

describe('DeterminateProgressSpinnerComponent', () => {
  let component: DeterminateProgressSpinnerComponent;
  let fixture: ComponentFixture<DeterminateProgressSpinnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeterminateProgressSpinnerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeterminateProgressSpinnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
