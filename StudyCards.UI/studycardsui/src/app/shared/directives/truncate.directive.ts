import { Directive, ElementRef, inject } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';

@Directive({
  selector: '[appTruncate]',
  hostDirectives: [{ directive: MatTooltip }]
})
export class TruncateDirective {
  private el = inject(ElementRef);
  private tooltip = inject(MatTooltip);
  private resizeObserver?: ResizeObserver;
  private textLabel?: HTMLElement;

  ngAfterViewInit(): void {
    const element = this.el.nativeElement;
    element.style.maxWidth = '100%';
   
    // Find and style the text label inside mat-chip
    this.textLabel = element.querySelector('.mdc-evolution-chip__text-label');
    if (this.textLabel) {
      Object.assign(this.textLabel.style, {
        overflow: 'hidden',
        textOverflow: 'ellipsis',
        whiteSpace: 'nowrap',
        display: 'block'
      });
    }
   
    // Observe size changes and check overflow
    this.resizeObserver = new ResizeObserver(() => this.checkOverflow());
    this.resizeObserver.observe(element);
    this.checkOverflow();
  }

  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();
  }

  private checkOverflow(): void {
    const target = this.textLabel || this.el.nativeElement;
    const isOverflowing = target.scrollWidth > target.clientWidth;
   
    this.tooltip.disabled = !isOverflowing;
    if (isOverflowing) {
      this.tooltip.message = this.el.nativeElement.textContent?.trim() || '';
    }
  }
}
