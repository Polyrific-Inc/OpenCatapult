import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DataModelPropertyNewDialogComponent } from './data-model-property-new-dialog.component';
import { DataModelPropertyFormComponent } from '../data-model-property-form/data-model-property-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule,
  MatDialogModule, MatSnackBarModule, MatDialogRef, MAT_DIALOG_DATA, MatSelectModule, MatDividerModule } from '@angular/material';
import { CoreModule } from '@app/core';
import { SnackbarService } from '@app/shared';
import { SharedModule } from '@app/shared/shared.module';

describe('DataModelPropertyNewDialogComponent', () => {
  let component: DataModelPropertyNewDialogComponent;
  let fixture: ComponentFixture<DataModelPropertyNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DataModelPropertyNewDialogComponent, DataModelPropertyFormComponent ],
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
        MatSelectModule,
        MatDividerModule,
        SharedModule.forRoot()
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
    fixture = TestBed.createComponent(DataModelPropertyNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
