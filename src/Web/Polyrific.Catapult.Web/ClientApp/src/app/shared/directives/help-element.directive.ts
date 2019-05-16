import { Directive, ElementRef, Input, Renderer2, OnInit } from '@angular/core';

@Directive({
  selector: '[appHelpElement]'
})
export class HelpElementDirective implements OnInit {
  @Input('appHelpElement') section: string;
  @Input() subSection: string;

  constructor(private el: ElementRef, private renderer: Renderer2) {
  }

   ngOnInit(): void {
    this.renderer.addClass(this.el.nativeElement, `help-element-${this.section}`);
    if (this.subSection) {
      this.renderer.setAttribute(this.el.nativeElement, 'data-sub-section', this.subSection);
    }
   }

}
