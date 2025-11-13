import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { DeckActionsComponent } from './deck-actions.component';

describe('DeckActionsComponent', () => {
  let component: DeckActionsComponent;
  let fixture: ComponentFixture<DeckActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeckActionsComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckActionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
