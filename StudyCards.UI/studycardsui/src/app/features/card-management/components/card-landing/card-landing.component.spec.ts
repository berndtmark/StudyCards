import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardLandingComponent } from './card-landing.component';
import { CardStore } from '../../store/card.store';
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
        CardStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardLandingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
