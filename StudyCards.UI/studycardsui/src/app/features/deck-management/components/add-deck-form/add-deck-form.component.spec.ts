import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeckFormComponent } from './add-deck-form.component';

describe('AddDeckFormComponent', () => {
  let component: AddDeckFormComponent;
  let fixture: ComponentFixture<AddDeckFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDeckFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
