import { Attribute, Component, Input, OnInit, ViewChild } from '@angular/core';
import { SwiperComponent } from 'swiper/angular';
import { DevpointMiniPreviewProps } from '@ui-kit/components/devpoint-mini-preview/devpoint-mini-preview.props';

@Component({
  selector: 'app-devpoint-swiper',
  templateUrl: './devpoint-swiper.component.html',
  styleUrls: ['./devpoint-swiper.component.css'],
})
export class DevpointSwiperComponent implements OnInit {
  @ViewChild(SwiperComponent) swiper?: SwiperComponent;
  @Input() items?: DevpointMiniPreviewProps[];
  @Input() public createLink?: string;
  @Input() routerParams?: any;
  @Input() public addLink?: boolean = true;
  @Input() label?: string;
  @Input() hide?: boolean = false;

  constructor() {}

  ngOnInit(): void {}

  swipePrev() {
    this.swiper?.swiperRef.slidePrev();
  }

  swipeNext() {
    this.swiper?.swiperRef.slideNext();
  }
}
