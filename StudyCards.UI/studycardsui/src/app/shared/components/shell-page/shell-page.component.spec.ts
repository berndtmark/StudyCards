import { ComponentFixture, TestBed } from '@angular/core/testing';

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
        provideHttpClientTesting()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShellPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
