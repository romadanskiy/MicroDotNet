import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDeveloperModalComponent } from './add-developer-modal.component';

describe('AddDeveloperModalComponent', () => {
  let component: AddDeveloperModalComponent;
  let fixture: ComponentFixture<AddDeveloperModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddDeveloperModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddDeveloperModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
