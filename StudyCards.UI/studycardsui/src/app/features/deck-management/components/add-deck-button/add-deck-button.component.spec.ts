import { ComponentFixture, TestBed } from '@angular/core/testing';
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
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
