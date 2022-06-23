import { ChangeDetectionStrategy, Component } from '@angular/core';
import { NuMarkdownBaseComponent } from '@ng-util/markdown';

declare var Vditor: any;

@Component({
  selector: 'app-markdown-preview',
  template: ``,
  exportAs: 'appMarkdownPreview',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppMarkdownPreviewComponent extends NuMarkdownBaseComponent {
  protected init(): void {
    this.ngZone.runOutsideAngular(async () => {
      const options = {
        hljs: {
          enable: false,
          lineNumber: false,
          style: 'github',
        },
        ...this.options,
      };
      await Vditor.preview(this.el.nativeElement, this._value, options);
      this.ngZone.run(() => this.ready.emit(this.el.nativeElement.innerHTML));
    });
  }
}
