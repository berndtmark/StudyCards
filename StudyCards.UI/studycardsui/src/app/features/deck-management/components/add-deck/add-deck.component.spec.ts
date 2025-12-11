import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeckComponent } from './add-deck.component';
import { RouterModule } from '@angular/router';
import { DeckStore } from '../../store/deck.store';

describe('AddDeckComponent', () => {
  let component: AddDeckComponent;
  let fixture: ComponentFixture<AddDeckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AddDeckComponent,
        RouterModule.forRoot([]),
      ],
      providers: [
        DeckStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
