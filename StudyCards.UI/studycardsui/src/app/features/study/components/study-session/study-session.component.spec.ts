import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudySessionComponent } from './study-session.component';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
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
        StudyStore,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudySessionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
