import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueLogComponent } from './job-queue-log.component';
import { JobQueueTaskStatusComponent } from '../components/job-queue-task-status/job-queue-task-status.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule, MatBadgeModule, MatButtonModule, MatDividerModule, MatDialogModule,
  MatChipsModule, MatProgressBarModule, MatInputModule, MatExpansionModule, MatProgressSpinnerModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SharedModule } from '@app/shared/shared.module';
import { CoreModule } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { JobQueueTaskLogComponent } from '../job-queue-task-log/job-queue-task-log.component';

describe('JobQueueLogComponent', () => {
  let component: JobQueueLogComponent;
  let fixture: ComponentFixture<JobQueueLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueLogComponent, JobQueueTaskStatusComponent, JobQueueTaskLogComponent ],
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
        MatInputModule,
        MatExpansionModule,
        MatProgressSpinnerModule
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
    fixture = TestBed.createComponent(JobQueueLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
