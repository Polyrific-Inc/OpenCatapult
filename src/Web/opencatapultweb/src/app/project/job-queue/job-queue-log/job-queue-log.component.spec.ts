import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueLogComponent } from './job-queue-log.component';

describe('JobQueueLogComponent', () => {
  let component: JobQueueLogComponent;
  let fixture: ComponentFixture<JobQueueLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueLogComponent ]
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
