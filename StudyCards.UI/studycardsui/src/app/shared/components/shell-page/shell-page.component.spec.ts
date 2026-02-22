import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShellPageComponent } from './shell-page.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RootStore } from '../../store/root.store';
import { provideRouter } from '@angular/router';

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
        provideRouter([])
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShellPageComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', async () => {
    expect(component).toBeTruthy();
  });
});
