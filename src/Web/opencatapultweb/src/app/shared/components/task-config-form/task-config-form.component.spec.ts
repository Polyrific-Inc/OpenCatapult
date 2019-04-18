import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskConfigFormComponent } from './task-config-form.component';
import { PullTaskConfigFormComponent } from '../pull-task-config-form/pull-task-config-form.component';
import { GenerateTaskConfigFormComponent } from '../generate-task-config-form/generate-task-config-form.component';
import { PushTaskConfigFormComponent } from '../push-task-config-form/push-task-config-form.component';
import { MergeTaskConfigFormComponent } from '../merge-task-config-form/merge-task-config-form.component';
import { BuildTaskConfigFormComponent } from '../build-task-config-form/build-task-config-form.component';
import { DeployDbTaskConfigFormComponent } from '../deploy-db-task-config-form/deploy-db-task-config-form.component';
import { DeployTaskConfigFormComponent } from '../deploy-task-config-form/deploy-task-config-form.component';
import { TestTaskConfigFormComponent } from '../test-task-config-form/test-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DeleteRepositoryConfigFormComponent } from '../delete-repository-config-form/delete-repository-config-form.component';

describe('TaskConfigFormComponent', () => {
  let component: TaskConfigFormComponent;
  let fixture: ComponentFixture<TaskConfigFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
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
        MatSelectModule
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskConfigFormComponent);
    component = fixture.componentInstance;
    component.task = {
      name: 'test',
      type: 'Clone',
      provider: 'test',
      configs: {},
      additionalConfigs: {},
      sequence: 1
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
