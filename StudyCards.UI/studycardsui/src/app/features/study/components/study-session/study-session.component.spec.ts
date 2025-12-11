import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudySessionComponent } from './study-session.component';
import { StudyStore } from '../../store/study.store';
import { RouterModule } from '@angular/router';

describe('StudySessionComponent', () => {
  let component: StudySessionComponent;
  let fixture: ComponentFixture<StudySessionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        StudySessionComponent,
        RouterModule.forRoot([]),
      ],
      providers: [
        StudyStore
      ]
    })
    .compileComponents();

  fixture = TestBed.createComponent(StudySessionComponent);
  component = fixture.componentInstance;
  await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
