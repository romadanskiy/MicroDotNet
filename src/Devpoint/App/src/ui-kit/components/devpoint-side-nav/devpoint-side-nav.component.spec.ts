import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevpointSideNavComponent } from './devpoint-side-nav.component';

describe('DevpointSideNavComponent', () => {
  let component: DevpointSideNavComponent;
  let fixture: ComponentFixture<DevpointSideNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevpointSideNavComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DevpointSideNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
