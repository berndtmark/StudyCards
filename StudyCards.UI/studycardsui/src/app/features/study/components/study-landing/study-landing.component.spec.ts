import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyLandingComponent } from './study-landing.component';

describe('StudyLandingComponent', () => {
  let component: StudyLandingComponent;
  let fixture: ComponentFixture<StudyLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudyLandingComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
