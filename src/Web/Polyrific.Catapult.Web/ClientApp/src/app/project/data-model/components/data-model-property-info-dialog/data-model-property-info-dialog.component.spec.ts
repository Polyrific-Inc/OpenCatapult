import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyInfoDialogComponent } from './data-model-property-info-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule, MatDialogModule,
  MatSnackBarModule, MatSelectModule, MatDividerModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CoreModule } from '@app/core';
import { SnackbarService } from '@app/shared';
import { DataModelPropertyFormComponent } from '../data-model-property-form/data-model-property-form.component';

describe('DataModelPropertyInfoDialogComponent', () => {
  let component: DataModelPropertyInfoDialogComponent;
  let fixture: ComponentFixture<DataModelPropertyInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyInfoDialogComponent, DataModelPropertyFormComponent ],
      imports: [
        BrowserAnimationsModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        MatProgressBarModule,
        MatDialogModule,
        MatSnackBarModule,
        CoreModule,
        MatCheckboxModule,
        MatSelectModule,
        MatDividerModule
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
    fixture = TestBed.createComponent(DataModelPropertyInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
