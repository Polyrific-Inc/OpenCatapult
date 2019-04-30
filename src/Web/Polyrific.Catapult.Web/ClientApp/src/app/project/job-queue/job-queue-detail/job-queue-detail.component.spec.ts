import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueDetailComponent } from './job-queue-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { JobQueueCancelDialogComponent } from '../components/job-queue-cancel-dialog/job-queue-cancel-dialog.component';
import { MatIconModule, MatBadgeModule, MatButtonModule,
  MatDividerModule, MatDialogModule, MatChipsModule, MatProgressBarModule, MatInputModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SharedModule } from '@app/shared/shared.module';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { CoreModule } from '@app/core';
import { JobQueueStatusComponent } from '../components/job-queue-status/job-queue-status.component';
import { ReactiveFormsModule } from '@angular/forms';
import { JobQueueTaskStatusComponent } from '../components/job-queue-task-status/job-queue-task-status.component';

describe('JobQueueDetailComponent', () => {
  let component: JobQueueDetailComponent;
  let fixture: ComponentFixture<JobQueueDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueDetailComponent, JobQueueCancelDialogComponent, JobQueueStatusComponent, JobQueueTaskStatusComponent ],
      imports: [
        RouterTestingModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        MatIconModule,
        MatBadgeModule,
        MatButtonModule,
        FlexLayoutModule,
        MatDividerModule,
        SharedModule.forRoot(),
        MatDialogModule,
        CoreModule,
        MatChipsModule,
        MatProgressBarModule,
        MatInputModule
      ],
      providers: [
        {
          provide: ActivatedRoute, useValue: {
            data: of({jobQueue: { id: 1, projectId: 1}}),
            snapshot: {}
          }
        }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
