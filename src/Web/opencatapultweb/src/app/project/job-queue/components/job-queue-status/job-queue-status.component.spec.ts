import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueStatusComponent } from './job-queue-status.component';

describe('JobQueueStatusComponent', () => {
  let component: JobQueueStatusComponent;
  let fixture: ComponentFixture<JobQueueStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
