import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { StudyCompleteComponent } from './study-complete.component';

describe('StudyCompleteComponent', () => {
  let component: StudyCompleteComponent;
  let fixture: ComponentFixture<StudyCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        StudyCompleteComponent,
      ],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyCompleteComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
