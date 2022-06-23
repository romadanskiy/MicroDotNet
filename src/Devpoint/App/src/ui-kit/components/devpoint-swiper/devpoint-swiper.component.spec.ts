import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevpointSwiperComponent } from './devpoint-swiper.component';

describe('SwiperComponent', () => {
  let component: DevpointSwiperComponent;
  let fixture: ComponentFixture<DevpointSwiperComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevpointSwiperComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DevpointSwiperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
