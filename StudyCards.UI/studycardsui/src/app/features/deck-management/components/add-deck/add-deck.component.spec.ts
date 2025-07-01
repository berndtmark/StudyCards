import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeckComponent } from './add-deck.component';
import { RouterModule } from '@angular/router';
import { DeckStore } from '../../store/deck.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

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
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
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
