import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudySessionActionsComponent } from './study-session-actions.component';

describe('StudySessionActionsComponent', () => {
  let component: StudySessionActionsComponent;
  let fixture: ComponentFixture<StudySessionActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudySessionActionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudySessionActionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
