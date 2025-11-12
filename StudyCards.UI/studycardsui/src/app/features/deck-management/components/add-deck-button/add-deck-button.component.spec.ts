import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';
import { DeckStore } from '../../store/deck.store';

import { AddDeckButtonComponent } from './add-deck-button.component';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('AddDeckComponent', () => {
  let component: AddDeckButtonComponent;
  let fixture: ComponentFixture<AddDeckButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDeckButtonComponent],
      providers: [
        DeckStore, 
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckButtonComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
