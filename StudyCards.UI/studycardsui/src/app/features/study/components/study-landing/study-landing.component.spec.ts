import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

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
      ],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyLandingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
