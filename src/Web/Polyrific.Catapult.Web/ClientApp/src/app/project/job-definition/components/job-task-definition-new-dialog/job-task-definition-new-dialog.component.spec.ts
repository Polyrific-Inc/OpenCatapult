import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionNewDialogComponent } from './job-task-definition-new-dialog.component';
import { JobTaskDefinitionFormComponent } from '../job-task-definition-form/job-task-definition-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatCheckboxModule, MatProgressBarModule, MatDialogModule,
  MatSnackBarModule, MatSelectModule, MatDividerModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CoreModule } from '@app/core';
import { SharedModule } from '@app/shared/shared.module';
import { SnackbarService } from '@app/shared';

describe('JobTaskDefinitionNewDialogComponent', () => {
  let component: JobTaskDefinitionNewDialogComponent;
  let fixture: ComponentFixture<JobTaskDefinitionNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionNewDialogComponent, JobTaskDefinitionFormComponent ],
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
    fixture = TestBed.createComponent(JobTaskDefinitionNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
