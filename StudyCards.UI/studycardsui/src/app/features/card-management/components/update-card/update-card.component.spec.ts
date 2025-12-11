import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateCardComponent } from './update-card.component';
import { CardStore } from '../../store/card.store';
import { RouterModule } from '@angular/router';

describe('UpdateCardComponent', () => {
  let component: UpdateCardComponent;
  let fixture: ComponentFixture<UpdateCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        UpdateCardComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        CardStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
