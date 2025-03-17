import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateDeckFormComponent } from './update-deck-form.component';
import { DeckStore } from 'app/features/deck-management/store/deck.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';

describe('UpdateDeckFormComponent', () => {
  let component: UpdateDeckFormComponent;
  let fixture: ComponentFixture<UpdateDeckFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UpdateDeckFormComponent,
        RouterModule.forRoot([]),
      ],
      providers: [
        DeckStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateDeckFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
