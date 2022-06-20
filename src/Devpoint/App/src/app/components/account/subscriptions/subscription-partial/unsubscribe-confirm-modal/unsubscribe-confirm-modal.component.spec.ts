import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnsubscribeConfirmModalComponent } from './unsubscribe-confirm-modal.component';

describe('SubscriptionConfirmModalComponent', () => {
  let component: UnsubscribeConfirmModalComponent;
  let fixture: ComponentFixture<UnsubscribeConfirmModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnsubscribeConfirmModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnsubscribeConfirmModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
