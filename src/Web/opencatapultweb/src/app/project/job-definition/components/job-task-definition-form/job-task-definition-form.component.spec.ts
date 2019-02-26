import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionFormComponent } from './job-task-definition-form.component';

describe('JobTaskDefinitionFormComponent', () => {
  let component: JobTaskDefinitionFormComponent;
  let fixture: ComponentFixture<JobTaskDefinitionFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobTaskDefinitionFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
