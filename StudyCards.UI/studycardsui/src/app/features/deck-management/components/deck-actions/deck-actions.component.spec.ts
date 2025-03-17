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
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
