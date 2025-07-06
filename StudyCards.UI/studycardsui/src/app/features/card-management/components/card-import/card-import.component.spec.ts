import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardImportComponent } from './card-import.component';
import { CardStore } from '../../store/card.store';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
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
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
