import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckItemComponent } from './deck-item.component';
import { DeckStore } from '../../store/deck.store';

describe('DeckItemComponent', () => {
  let component: DeckItemComponent;
  let fixture: ComponentFixture<DeckItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeckItemComponent],
      providers: [
        DeckStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckItemComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('deck', { deckName: 'my deck' });

    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
