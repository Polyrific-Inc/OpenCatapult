import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueCancelDialogComponent } from './job-queue-cancel-dialog.component';
import { MatButtonModule, MatInputModule, MatProgressBarModule, MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('JobQueueCancelDialogComponent', () => {
  let component: JobQueueCancelDialogComponent;
  let fixture: ComponentFixture<JobQueueCancelDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueCancelDialogComponent ],
      imports: [
        BrowserAnimationsModule,
        MatButtonModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatInputModule,
        MatProgressBarModule,
        MatDialogModule,
        SharedModule.forRoot(),
        CoreModule
      ],
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
    fixture = TestBed.createComponent(JobQueueCancelDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
