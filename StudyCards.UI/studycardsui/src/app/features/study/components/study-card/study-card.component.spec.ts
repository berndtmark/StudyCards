import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudyCardComponent } from './study-card.component';

describe('StudyCardComponent', () => {
  let component: StudyCardComponent;
  let fixture: ComponentFixture<StudyCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudyCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudyCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
