import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCardFormComponent } from './add-card-form.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';
import { CardStore } from 'app/features/card-management/store/card.store';

describe('AddCardFormComponent', () => {
  let component: AddCardFormComponent;
  let fixture: ComponentFixture<AddCardFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AddCardFormComponent,
        RouterModule.forRoot([])
      ],
      providers: [
        CardStore,
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCardFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
