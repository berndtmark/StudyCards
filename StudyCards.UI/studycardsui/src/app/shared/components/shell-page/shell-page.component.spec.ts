import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { ShellPageComponent } from './shell-page.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RootStore } from 'app/shared/store/root.store';

describe('ShellPageComponent', () => {
  let component: ShellPageComponent;
  let fixture: ComponentFixture<ShellPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShellPageComponent],
      providers: [
        RootStore,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShellPageComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
