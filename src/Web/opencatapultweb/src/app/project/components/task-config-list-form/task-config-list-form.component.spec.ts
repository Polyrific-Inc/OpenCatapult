import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskConfigListFormComponent } from './task-config-list-form.component';
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
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule, MatStepperModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('TaskConfigListFormComponent', () => {
  let component: TaskConfigListFormComponent;
  let fixture: ComponentFixture<TaskConfigListFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TaskConfigListFormComponent,
        TaskConfigFormComponent,
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
        BrowserAnimationsModule,
        MatExpansionModule,
        ReactiveFormsModule,
        MatInputModule,
        MatCheckboxModule,
        CoreModule,
        HttpClientTestingModule,
        MatSelectModule,
        MatStepperModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskConfigListFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
