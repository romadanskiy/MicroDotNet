import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscriptionPartialComponent } from './subscription-partial.component';

describe('SubscriptionPartialComponent', () => {
  let component: SubscriptionPartialComponent;
  let fixture: ComponentFixture<SubscriptionPartialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscriptionPartialComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubscriptionPartialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
