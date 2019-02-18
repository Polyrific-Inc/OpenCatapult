import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobListFormComponent } from './job-list-form.component';

describe('JobListFormComponent', () => {
  let component: JobListFormComponent;
  let fixture: ComponentFixture<JobListFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobListFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobListFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
