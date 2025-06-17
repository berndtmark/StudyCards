import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsToStudyComponent } from './cards-to-study.component';
import { StudyStore } from '../../store/study.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('CardsToStudyComponent', () => {
  let component: CardsToStudyComponent;
  let fixture: ComponentFixture<CardsToStudyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardsToStudyComponent],
      providers: [
        StudyStore,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardsToStudyComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('cardsToStudy', [{ id: '123' }]);

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
