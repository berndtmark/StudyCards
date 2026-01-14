import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardImportDisplayComponent } from './card-import-display.component';

describe('CardImportDisplayComponent', () => {
  let component: CardImportDisplayComponent;
  let fixture: ComponentFixture<CardImportDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardImportDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportDisplayComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
