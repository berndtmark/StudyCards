import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyCardActionsComponent } from './study-card-actions.component';

describe('StudyCardActionsComponent', () => {
  let component: StudyCardActionsComponent;
  let fixture: ComponentFixture<StudyCardActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudyCardActionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyCardActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
