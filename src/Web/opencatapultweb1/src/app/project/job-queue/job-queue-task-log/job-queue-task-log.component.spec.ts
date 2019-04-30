import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueTaskLogComponent } from './job-queue-task-log.component';
import { MatIconModule, MatBadgeModule, MatButtonModule, MatChipsModule,
  MatDividerModule, MatListModule, MatExpansionModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SharedModule } from '@app/shared/shared.module';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CoreModule } from '@app/core';
import { JobQueueTaskStatusComponent } from '../components/job-queue-task-status/job-queue-task-status.component';

describe('JobQueueTaskLogComponent', () => {
  let component: JobQueueTaskLogComponent;
  let fixture: ComponentFixture<JobQueueTaskLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueTaskLogComponent, JobQueueTaskStatusComponent ],
      imports: [
        HttpClientTestingModule,
        MatIconModule,
        MatBadgeModule,
        MatButtonModule,
        FlexLayoutModule,
        MatChipsModule,
        MatDividerModule,
        MatListModule,
        SharedModule.forRoot(),
        CoreModule,
        MatExpansionModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueTaskLogComponent);
    component = fixture.componentInstance;
    component.taskStatus = {
      sequence: 1,
      taskName: 'test',
      status: 'NotExecuted',
      remarks: ''
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
