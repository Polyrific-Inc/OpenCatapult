import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueTaskStatusComponent } from './job-queue-task-status.component';
import { MatChipsModule } from '@angular/material';

describe('JobQueueTaskStatusComponent', () => {
  let component: JobQueueTaskStatusComponent;
  let fixture: ComponentFixture<JobQueueTaskStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueTaskStatusComponent ],
      imports: [
        MatChipsModule
      ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueTaskStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
