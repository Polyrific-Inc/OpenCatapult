import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskConfigFormComponent } from './components/task-config-form/task-config-form.component';
import { JobConfigFormComponent } from './components/job-config-form/job-config-form.component';
import { MatDividerModule, MatSnackBarModule, MatFormFieldModule, MatInputModule, MatCheckboxModule, MatExpansionModule } from '@angular/material';
import { TaskConfigListFormComponent } from './components/task-config-list-form/task-config-list-form.component';
import { BuildTaskConfigFormComponent } from './components/build-task-config-form/build-task-config-form.component';
import { CloneTaskConfigFormComponent } from './components/clone-task-config-form/clone-task-config-form.component';
import { SnackbarService } from './services/snackbar.service';
import { ReactiveFormsModule } from '@angular/forms';
import { GenerateTaskConfigFormComponent } from './components/generate-task-config-form/generate-task-config-form.component';
import { PushTaskConfigFormComponent } from './components/push-task-config-form/push-task-config-form.component';
import { MergeTaskConfigFormComponent } from './components/merge-task-config-form/merge-task-config-form.component';
import { DeployDbTaskConfigFormComponent } from './components/deploy-db-task-config-form/deploy-db-task-config-form.component';
import { DeployTaskConfigFormComponent } from './components/deploy-task-config-form/deploy-task-config-form.component';
import { AdditionalConfigFormComponent } from './components/additional-config-form/additional-config-form.component';
import { TestTaskConfigFormComponent } from './components/test-task-config-form/test-task-config-form.component';

@NgModule({
  declarations: [TaskConfigFormComponent, JobConfigFormComponent, TaskConfigListFormComponent, BuildTaskConfigFormComponent, CloneTaskConfigFormComponent, GenerateTaskConfigFormComponent, PushTaskConfigFormComponent, MergeTaskConfigFormComponent, DeployDbTaskConfigFormComponent, DeployTaskConfigFormComponent, AdditionalConfigFormComponent, TestTaskConfigFormComponent],
  imports: [
    CommonModule,
    MatDividerModule,
    MatSnackBarModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatExpansionModule
  ],
  exports: [
    TaskConfigFormComponent,
    JobConfigFormComponent, 
    TaskConfigListFormComponent, 
    BuildTaskConfigFormComponent, 
    CloneTaskConfigFormComponent,
    GenerateTaskConfigFormComponent,
    PushTaskConfigFormComponent,
    MergeTaskConfigFormComponent,
    BuildTaskConfigFormComponent,
    DeployTaskConfigFormComponent,
    DeployDbTaskConfigFormComponent,
    TestTaskConfigFormComponent,
  ]
})
export class SharedModule {
  static forRoot() {
    return {
      ngModule: SharedModule,
      providers: [
        SnackbarService
      ]
    }
  }
 }
