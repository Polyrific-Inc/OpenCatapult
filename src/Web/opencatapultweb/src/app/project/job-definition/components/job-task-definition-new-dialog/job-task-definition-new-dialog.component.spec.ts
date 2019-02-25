import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionNewDialogComponent } from './job-task-definition-new-dialog.component';

describe('JobTaskDefinitionNewDialogComponent', () => {
  let component: JobTaskDefinitionNewDialogComponent;
  let fixture: ComponentFixture<JobTaskDefinitionNewDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionNewDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobTaskDefinitionNewDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
