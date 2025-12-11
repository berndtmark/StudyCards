import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckActionsComponent } from './deck-actions.component';

describe('DeckActionsComponent', () => {
  let component: DeckActionsComponent;
  let fixture: ComponentFixture<DeckActionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeckActionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckActionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
