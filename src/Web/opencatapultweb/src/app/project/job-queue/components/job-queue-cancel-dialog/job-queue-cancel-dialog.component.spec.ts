import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueCancelDialogComponent } from './job-queue-cancel-dialog.component';

describe('JobQueueCancelDialogComponent', () => {
  let component: JobQueueCancelDialogComponent;
  let fixture: ComponentFixture<JobQueueCancelDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueCancelDialogComponent ]
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
