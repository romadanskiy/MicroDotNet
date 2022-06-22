import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevpointMiniPreviewComponent } from './devpoint-mini-preview.component';

describe('ProjectMiniPreviewComponent', () => {
  let component: DevpointMiniPreviewComponent;
  let fixture: ComponentFixture<DevpointMiniPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevpointMiniPreviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DevpointMiniPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
