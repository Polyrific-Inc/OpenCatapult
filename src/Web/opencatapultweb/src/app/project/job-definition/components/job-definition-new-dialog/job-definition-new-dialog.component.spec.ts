import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobDefinitionNewDialogComponent } from './job-definition-new-dialog.component';
import { JobDefinitionFormComponent } from '../job-definition-form/job-definition-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule, MatDialogModule,
  MatSnackBarModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CoreModule } from '@app/core';
import { SnackbarService } from '@app/shared';

describe('JobDefinitionNewDialogComponent', () => {
  let component: JobDefinitionNewDialogComponent;
  let fixture: ComponentFixture<JobDefinitionNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobDefinitionNewDialogComponent, JobDefinitionFormComponent ],
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
    fixture = TestBed.createComponent(JobDefinitionNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
