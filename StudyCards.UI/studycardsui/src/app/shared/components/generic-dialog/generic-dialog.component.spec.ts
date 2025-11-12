import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { GenericDialogComponent } from './generic-dialog.component';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('GenericDialogComponent', () => {
  let component: GenericDialogComponent;
  let fixture: ComponentFixture<GenericDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        provideZonelessChangeDetection()
    ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenericDialogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
