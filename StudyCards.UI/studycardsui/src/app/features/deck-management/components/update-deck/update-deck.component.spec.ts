import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDeckComponent } from './update-deck.component';
import { RouterModule } from '@angular/router';
import { DeckStore } from '../../store/deck.store';

describe('UpdateDeckComponent', () => {
  let component: UpdateDeckComponent;
  let fixture: ComponentFixture<UpdateDeckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UpdateDeckComponent,
        RouterModule.forRoot([]),
      ],
      providers: [
        DeckStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDeckComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
