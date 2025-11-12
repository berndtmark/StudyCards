import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { AddCardComponent } from './add-card.component';
import { RouterModule } from '@angular/router';
import { CardStore } from '../../store/card.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('AddCardComponent', () => {
  let component: AddCardComponent;
  let fixture: ComponentFixture<AddCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AddCardComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        provideZonelessChangeDetection(),
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable()
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
