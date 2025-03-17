import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeckFormComponent } from './add-deck-form.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { DeckStore } from 'app/features/deck-management/store/deck.store';

describe('AddDeckFormComponent', () => {
  let component: AddDeckFormComponent;
  let fixture: ComponentFixture<AddDeckFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddDeckFormComponent],
      providers: [
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDeckFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
