import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCardComponent } from './add-card.component';
import { RouterModule } from '@angular/router';
import { CardStore } from '../../store/card.store';

describe('AddCardComponent', () => {
  let component: AddCardComponent;
  let fixture: ComponentFixture<AddCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AddCardComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        CardStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable()
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
