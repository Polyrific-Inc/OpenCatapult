import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskConfigListFormComponent } from './task-config-list-form.component';
import { TaskConfigFormComponent } from '@app/shared';
import { CloneTaskConfigFormComponent } from '../clone-task-config-form/clone-task-config-form.component';
import { GenerateTaskConfigFormComponent } from '../generate-task-config-form/generate-task-config-form.component';
import { PushTaskConfigFormComponent } from '../push-task-config-form/push-task-config-form.component';
import { MergeTaskConfigFormComponent } from '../merge-task-config-form/merge-task-config-form.component';
import { BuildTaskConfigFormComponent } from '../build-task-config-form/build-task-config-form.component';
import { DeployDbTaskConfigFormComponent } from '../deploy-db-task-config-form/deploy-db-task-config-form.component';
import { DeployTaskConfigFormComponent } from '../deploy-task-config-form/deploy-task-config-form.component';
import { TestTaskConfigFormComponent } from '../test-task-config-form/test-task-config-form.component';
import { AdditionalConfigFormComponent } from '../additional-config-form/additional-config-form.component';
import { AdditionalConfigFieldComponent } from '../additional-config-field/additional-config-field.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatExpansionModule, MatInputModule, MatCheckboxModule, MatSelectModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DeleteRepositoryConfigFormComponent } from '../delete-repository-config-form/delete-repository-config-form.component';

describe('TaskConfigListFormComponent', () => {
  let component: TaskConfigListFormComponent;
  let fixture: ComponentFixture<TaskConfigListFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        TaskConfigListFormComponent,
        TaskConfigFormComponent,
        TaskConfigFormComponent,
        CloneTaskConfigFormComponent,
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
    fixture = TestBed.createComponent(TaskConfigListFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
