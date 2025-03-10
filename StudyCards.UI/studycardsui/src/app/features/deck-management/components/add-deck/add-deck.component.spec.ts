import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeckStore } from '../../store/deck.store';

import { AddDeckComponent } from './add-deck.component';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('AddDeckComponent', () => {
  let component: AddDeckComponent;
  let fixture: ComponentFixture<AddDeckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDeckComponent],
      providers: [
        DeckStore, 
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
