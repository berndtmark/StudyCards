import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateCardFormComponent } from './update-card-form.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { DeckStore } from 'app/features/deck-management/store/deck.store';

describe('UpdateCardFormComponent', () => {
  let component: UpdateCardFormComponent;
  let fixture: ComponentFixture<UpdateCardFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateCardFormComponent],
      providers: [
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateCardFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
