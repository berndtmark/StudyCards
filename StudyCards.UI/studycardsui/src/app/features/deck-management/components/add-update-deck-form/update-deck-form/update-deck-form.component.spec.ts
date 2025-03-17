import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDeckFormComponent } from './update-deck-form.component';

describe('UpdateDeckFormComponent', () => {
  let component: UpdateDeckFormComponent;
  let fixture: ComponentFixture<UpdateDeckFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateDeckFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDeckFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
