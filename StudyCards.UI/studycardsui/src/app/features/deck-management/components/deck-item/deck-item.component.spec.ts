import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { DeckItemComponent } from './deck-item.component';
import { DeckStore } from '../../store/deck.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('DeckItemComponent', () => {
  let component: DeckItemComponent;
  let fixture: ComponentFixture<DeckItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeckItemComponent],
      providers: [
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckItemComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('deck', { deckName: 'my deck' });

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
