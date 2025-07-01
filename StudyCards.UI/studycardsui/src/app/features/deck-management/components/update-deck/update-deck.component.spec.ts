import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDeckComponent } from './update-deck.component';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';
import { DeckStore } from '../../store/deck.store';
import { provideHttpClient } from '@angular/common/http';

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
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDeckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
