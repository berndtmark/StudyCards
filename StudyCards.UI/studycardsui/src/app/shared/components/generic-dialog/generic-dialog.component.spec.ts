import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericDialogComponent } from './generic-dialog.component';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('GenericDialogComponent', () => {
  let component: GenericDialogComponent;
  let fixture: ComponentFixture<GenericDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GenericDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GenericDialogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
