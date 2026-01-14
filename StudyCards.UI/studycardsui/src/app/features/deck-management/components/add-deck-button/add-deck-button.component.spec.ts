import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeckStore } from '../../store/deck.store';

import { AddDeckButtonComponent } from './add-deck-button.component';

describe('AddDeckComponent', () => {
  let component: AddDeckButtonComponent;
  let fixture: ComponentFixture<AddDeckButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDeckButtonComponent],
      providers: [
        DeckStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckButtonComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
