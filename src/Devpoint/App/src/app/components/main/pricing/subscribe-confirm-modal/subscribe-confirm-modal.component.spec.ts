import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscribeConfirmModalComponent } from './subscribe-confirm-modal.component';

describe('SubscribeConfirmModalComponent', () => {
  let component: SubscribeConfirmModalComponent;
  let fixture: ComponentFixture<SubscribeConfirmModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscribeConfirmModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscribeConfirmModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
