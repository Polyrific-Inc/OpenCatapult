import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobTaskDefinitionInfoDialogComponent } from './job-task-definition-info-dialog.component';

describe('JobTaskDefinitionInfoDialogComponent', () => {
  let component: JobTaskDefinitionInfoDialogComponent;
  let fixture: ComponentFixture<JobTaskDefinitionInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobTaskDefinitionInfoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobTaskDefinitionInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
