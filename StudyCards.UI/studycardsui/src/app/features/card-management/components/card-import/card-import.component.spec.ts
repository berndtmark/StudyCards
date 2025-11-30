import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardImportComponent } from './card-import.component';
import { CardStore } from '../../store/card.store';
import { RouterModule } from '@angular/router';

describe('CardImportComponent', () => {
  let component: CardImportComponent;
  let fixture: ComponentFixture<CardImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        CardImportComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        CardStore
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
