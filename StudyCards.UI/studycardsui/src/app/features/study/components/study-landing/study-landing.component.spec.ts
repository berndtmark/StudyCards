import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyLandingComponent } from './study-landing.component';
import { RouterModule } from '@angular/router';

describe('StudyLandingComponent', () => {
  let component: StudyLandingComponent;
  let fixture: ComponentFixture<StudyLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        StudyLandingComponent,
        RouterModule.forRoot([])
      ]
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
