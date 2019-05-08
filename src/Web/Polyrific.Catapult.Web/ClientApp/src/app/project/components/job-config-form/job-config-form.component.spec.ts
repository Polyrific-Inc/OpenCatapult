import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobConfigFormComponent } from './job-config-form.component';

import {
  TaskConfigFormComponent,
  PullTaskConfigFormComponent,
  GenerateTaskConfigFormComponent,
  PushTaskConfigFormComponent,
  MergeTaskConfigFormComponent,
  BuildTaskConfigFormComponent,
  DeployDbTaskConfigFormComponent,
  DeployTaskConfigFormComponent,
  TestTaskConfigFormComponent,
  AdditionalConfigFieldComponent,
  AdditionalConfigFormComponent,
  DeleteRepositoryConfigFormComponent
 } from '@app/shared';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule, MatStepperModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { TaskConfigListFormComponent } from '../task-config-list-form/task-config-list-form.component';

describe('JobConfigFormComponent', () => {
  let component: JobConfigFormComponent;
  let fixture: ComponentFixture<JobConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        JobConfigFormComponent,
        TaskConfigListFormComponent,
        TaskConfigFormComponent,
        PullTaskConfigFormComponent,
        GenerateTaskConfigFormComponent,
        PushTaskConfigFormComponent,
        MergeTaskConfigFormComponent,
        BuildTaskConfigFormComponent,
        DeployDbTaskConfigFormComponent,
        DeployTaskConfigFormComponent,
        TestTaskConfigFormComponent,
        AdditionalConfigFormComponent,
        AdditionalConfigFieldComponent,
        DeleteRepositoryConfigFormComponent
      ],
      imports: [ MatExpansionModule, ReactiveFormsModule, MatInputModule, MatCheckboxModule, MatSelectModule, MatStepperModule ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobConfigFormComponent);
    component = fixture.componentInstance;
    component.job = {
      name: 'test',
      tasks: [],
      isDeletion: false
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
