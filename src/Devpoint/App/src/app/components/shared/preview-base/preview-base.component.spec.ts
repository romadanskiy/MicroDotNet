import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewBaseComponent } from './preview-base.component';

describe('PreviewBaseComponent', () => {
  let component: PreviewBaseComponent;
  let fixture: ComponentFixture<PreviewBaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreviewBaseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewBaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
