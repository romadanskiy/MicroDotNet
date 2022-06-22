import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevpointUploadImageComponent } from './devpoint-upload-image.component';

describe('DevpointUploadImageComponent', () => {
  let component: DevpointUploadImageComponent;
  let fixture: ComponentFixture<DevpointUploadImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevpointUploadImageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DevpointUploadImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
