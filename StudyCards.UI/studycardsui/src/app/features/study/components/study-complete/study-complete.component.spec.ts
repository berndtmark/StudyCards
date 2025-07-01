import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyCompleteComponent } from './study-complete.component';

describe('StudyCompleteComponent', () => {
  let component: StudyCompleteComponent;
  let fixture: ComponentFixture<StudyCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        StudyCompleteComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
