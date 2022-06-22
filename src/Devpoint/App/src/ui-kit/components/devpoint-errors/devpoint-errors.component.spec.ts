import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevpointErrorsComponent } from './devpoint-errors.component';

describe('DevpointErrorsComponent', () => {
  let component: DevpointErrorsComponent;
  let fixture: ComponentFixture<DevpointErrorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DevpointErrorsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DevpointErrorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
