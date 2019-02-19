import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationWithInputDialogComponent } from './confirmation-with-input-dialog.component';

describe('ConfirmationWithInputDialogComponent', () => {
  let component: ConfirmationWithInputDialogComponent;
  let fixture: ComponentFixture<ConfirmationWithInputDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmationWithInputDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmationWithInputDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
