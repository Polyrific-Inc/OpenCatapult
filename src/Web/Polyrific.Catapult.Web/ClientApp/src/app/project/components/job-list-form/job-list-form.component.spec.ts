import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobListFormComponent } from './job-list-form.component';
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
  DeleteRepositoryConfigFormComponent } from '@app/shared';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule, MatStepperModule,
  MatTabsModule, MatDividerModule, MatListModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';

describe('JobListFormComponent', () => {
  let component: JobListFormComponent;
  let fixture: ComponentFixture<JobListFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        JobListFormComponent,
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
      imports: [
        MatExpansionModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        MatSelectModule,
        MatStepperModule,
        MatTabsModule,
        MatDividerModule,
        MatListModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobListFormComponent);
    component = fixture.componentInstance;
    component.jobDefinitions = [];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
