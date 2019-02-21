import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelInfoDialogComponent } from './data-model-info-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule,
  MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatSnackBarModule } from '@angular/material';
import { DataModelFormComponent } from '../data-model-form/data-model-form.component';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SnackbarService } from '@app/shared';

describe('DataModelInfoDialogComponent', () => {
  let component: DataModelInfoDialogComponent;
  let fixture: ComponentFixture<DataModelInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelInfoDialogComponent, DataModelFormComponent ],
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
    fixture = TestBed.createComponent(DataModelInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
