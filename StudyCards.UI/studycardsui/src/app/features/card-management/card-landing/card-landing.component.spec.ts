import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardLandingComponent } from './card-landing.component';
import { CardStore } from '../store/card.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';

describe('CardLandingComponent', () => {
  let component: CardLandingComponent;
  let fixture: ComponentFixture<CardLandingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        CardLandingComponent,
        RouterModule.forRoot([]),
      ],
      providers: [
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardLandingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
