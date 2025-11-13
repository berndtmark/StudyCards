import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { CardImportComponent } from './card-import.component';
import { CardStore } from '../../store/card.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';

describe('CardImportComponent', () => {
  let component: CardImportComponent;
  let fixture: ComponentFixture<CardImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        CardImportComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
