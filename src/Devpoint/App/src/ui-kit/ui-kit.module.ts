import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {
  DevpointButtonComponent,
  DevpointErrorsComponent,
  DevpointMiniPreviewComponent,
  DevpointSideNavComponent,
  DevpointSwiperComponent,
  DevpointUploadImageComponent,
} from './components';
import { AppRoutingModule } from '../app/app-routing.module';
import { SwiperModule } from 'swiper/angular';
import { RangePipe } from '@ui-kit/range.pipe';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DndDirective } from '@ui-kit/dnd.directive';

const components = [
  DevpointButtonComponent,
  DevpointSwiperComponent,
  DevpointMiniPreviewComponent,
  DevpointUploadImageComponent,
  DevpointSideNavComponent,
  DevpointErrorsComponent,
];

@NgModule({
  imports: [
    BrowserModule,
    AppRoutingModule,
    SwiperModule,
    MatProgressSpinnerModule,
  ],
  declarations: [...components, RangePipe, DndDirective],
  exports: [...components, RangePipe, DndDirective],
})
export class UiKitModule {}
