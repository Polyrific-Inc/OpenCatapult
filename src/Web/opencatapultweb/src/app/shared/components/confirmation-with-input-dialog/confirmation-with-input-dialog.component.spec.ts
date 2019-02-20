import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationWithInputDialogComponent } from './confirmation-with-input-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ConfirmationWithInputDialogComponent', () => {
  let component: ConfirmationWithInputDialogComponent;
  let fixture: ComponentFixture<ConfirmationWithInputDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmationWithInputDialogComponent ],
      imports: [ BrowserAnimationsModule, ReactiveFormsModule, MatInputModule ],
      providers: [
        {
          provide: MatDialogRef, useValue: {
            close: function (result) {

            }
          }
        },
        {
          provide: MAT_DIALOG_DATA, useValue: {

          }
        }
      ]
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
