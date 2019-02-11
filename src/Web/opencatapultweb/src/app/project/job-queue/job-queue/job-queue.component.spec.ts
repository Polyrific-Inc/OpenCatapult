import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobQueueComponent } from './job-queue.component';

describe('JobQueueComponent', () => {
  let component: JobQueueComponent;
  let fixture: ComponentFixture<JobQueueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobQueueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobQueueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
