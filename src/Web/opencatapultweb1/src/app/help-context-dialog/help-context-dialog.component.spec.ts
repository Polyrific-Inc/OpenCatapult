import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpContextDialogComponent } from './help-context-dialog.component';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

describe('HelpContextDialogComponent', () => {
  let component: HelpContextDialogComponent;
  let fixture: ComponentFixture<HelpContextDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HelpContextDialogComponent ],
      imports: [
        MatDialogModule
      ],
      providers: [
        {
          provide: MatDialogRef, useValue: {
            close: function (result) {

            }
          }
        },
        {
          provide: MAT_DIALOG_DATA, useValue: []
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpContextDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
