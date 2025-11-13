import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { StudyCardActionsComponent } from './study-card-actions.component';

describe('StudyCardActionsComponent', () => {
  let component: StudyCardActionsComponent;
  let fixture: ComponentFixture<StudyCardActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudyCardActionsComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyCardActionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
