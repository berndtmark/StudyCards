import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { CardImportDisplayComponent } from './card-import-display.component';

describe('CardImportDisplayComponent', () => {
  let component: CardImportDisplayComponent;
  let fixture: ComponentFixture<CardImportDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardImportDisplayComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportDisplayComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
