import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TruncateDirective } from './truncate.directive';
import { Component } from '@angular/core';

vi.stubGlobal('ResizeObserver', class ResizeObserver {
  observe = vi.fn();
  unobserve = vi.fn();
  disconnect = vi.fn();
});

// Create a fake component just for the test
@Component({
  template: `<div appTruncate=".text-target" style="width: 50px;">
               <span class="text-target">This is a really long string that should overflow</span>
             </div>`,
  standalone: true,
  imports: [TruncateDirective] // Import your directive here
})
class HostComponent {}

describe('TruncateDirective', () => {
  let component: HostComponent;
  let fixture: ComponentFixture<HostComponent>;
  
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
    })
    .compileComponents();

    // Create the fake component
    fixture = TestBed.createComponent(HostComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create an instance', () => {
    expect(component).toBeTruthy();
  });
});