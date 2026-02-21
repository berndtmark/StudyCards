import { Directive, ElementRef, inject, input, AfterViewInit, OnDestroy } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';

@Directive({
  selector: '[appTruncate]',
  hostDirectives: [{ directive: MatTooltip }]
})
export class TruncateDirective implements AfterViewInit, OnDestroy {
  targetClass = input<string | undefined>(undefined, { alias: 'appTruncate' });

  private el = inject(ElementRef);
  private tooltip = inject(MatTooltip);
  private resizeObserver?: ResizeObserver;
  private textElement?: HTMLElement;

  ngAfterViewInit(): void {
    const host = this.el.nativeElement;
    host.style.maxWidth = '100%';

    const targetSelector = this.targetClass();

    this.textElement = targetSelector 
      ? host.querySelector(targetSelector) as HTMLElement 
      : host;

    if (this.textElement) {
      Object.assign(this.textElement.style, {
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        whiteSpace: 'nowrap',
        display: 'block'
      });
    }
   
    // Observe size changes and check overflow
    this.resizeObserver = new ResizeObserver(() => this.checkOverflow());
    this.resizeObserver.observe(host);
    this.checkOverflow();
  }

  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();
  }

  private checkOverflow(): void {
    if (!this.textElement) return;

    const isOverflowing = this.textElement.scrollWidth > this.textElement.clientWidth;
   
    this.tooltip.disabled = !isOverflowing;
    if (isOverflowing) {
      this.tooltip.message = this.el.nativeElement.textContent?.trim() || '';
    }
  }
}