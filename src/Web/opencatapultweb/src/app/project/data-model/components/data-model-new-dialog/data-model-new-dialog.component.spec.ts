import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelNewDialogComponent } from './data-model-new-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule,
  MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatSnackBarModule } from '@angular/material';
import { DataModelFormComponent } from '../data-model-form/data-model-form.component';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SnackbarService } from '@app/shared';

describe('DataModelNewDialogComponent', () => {
  let component: DataModelNewDialogComponent;
  let fixture: ComponentFixture<DataModelNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelNewDialogComponent, DataModelFormComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatDialogModule,
        MatSnackBarModule,
        CoreModule
      ],
      providers: [
        SnackbarService,
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
    fixture = TestBed.createComponent(DataModelNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
