import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueDetailComponent } from './job-queue-detail.component';

describe('JobQueueDetailComponent', () => {
  let component: JobQueueDetailComponent;
  let fixture: ComponentFixture<JobQueueDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueDetailComponent ]
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
