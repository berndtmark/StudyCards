import { TestBed } from '@angular/core/testing';
import { TruncateDirective } from './truncate.directive';
import { ElementRef, ViewContainerRef } from '@angular/core';
import { MatTooltipModule, MatTooltip } from '@angular/material/tooltip';

describe('TruncateDirective', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [MatTooltipModule],
      providers: [
        { provide: ElementRef, useValue: { nativeElement: document.createElement('div') } },
        { provide: ViewContainerRef, useValue: {} },
        MatTooltip
      ]
    });
  });

  it('should create an instance', () => {
    // ✅ Use runInInjectionContext so inject() works
    TestBed.runInInjectionContext(() => {
      const directive = new TruncateDirective();
      expect(directive).toBeTruthy();
    });
  });
});