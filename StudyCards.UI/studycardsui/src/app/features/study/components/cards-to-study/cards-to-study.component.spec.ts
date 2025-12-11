import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsToStudyComponent } from './cards-to-study.component';

describe('CardsToStudyComponent', () => {
  let component: CardsToStudyComponent;
  let fixture: ComponentFixture<CardsToStudyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardsToStudyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardsToStudyComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('cardsToStudy', [{ id: '123' }]);

    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
