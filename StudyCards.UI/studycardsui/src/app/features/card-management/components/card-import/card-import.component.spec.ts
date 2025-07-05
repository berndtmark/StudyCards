import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardImportComponent } from './card-import.component';

describe('CardImportComponent', () => {
  let component: CardImportComponent;
  let fixture: ComponentFixture<CardImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardImportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
