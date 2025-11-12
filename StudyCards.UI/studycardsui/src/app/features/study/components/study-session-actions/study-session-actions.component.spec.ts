import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { StudySessionActionsComponent } from './study-session-actions.component';

describe('StudySessionActionsComponent', () => {
  let component: StudySessionActionsComponent;
  let fixture: ComponentFixture<StudySessionActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudySessionActionsComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudySessionActionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
