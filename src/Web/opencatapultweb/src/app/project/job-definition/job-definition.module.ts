import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobDefinitionRoutingModule } from './job-definition-routing.module';
import { JobDefinitionComponent } from './job-definition/job-definition.component';
import { JobTaskDefinitionComponent } from './job-task-definition/job-task-definition.component';
import { JobDefinitionNewDialogComponent } from './components/job-definition-new-dialog/job-definition-new-dialog.component';
import { MatButtonModule, MatExpansionModule, MatListModule, MatIconModule,
  MatCheckboxModule, MatInputModule, MatDialogModule, MatProgressBarModule, MatSelectModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { JobDefinitionInfoDialogComponent } from './components/job-definition-info-dialog/job-definition-info-dialog.component';
import { JobTaskDefinitionNewDialogComponent } from './components/job-task-definition-new-dialog/job-task-definition-new-dialog.component';
import {
  JobTaskDefinitionInfoDialogComponent
} from './components/job-task-definition-info-dialog/job-task-definition-info-dialog.component';
import { JobDefinitionFormComponent } from './components/job-definition-form/job-definition-form.component';
import { JobTaskDefinitionFormComponent } from './components/job-task-definition-form/job-task-definition-form.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    JobDefinitionComponent,
    JobTaskDefinitionComponent,
    JobDefinitionNewDialogComponent,
    JobDefinitionInfoDialogComponent,
    JobTaskDefinitionNewDialogComponent,
    JobTaskDefinitionInfoDialogComponent,
    JobDefinitionFormComponent,
    JobTaskDefinitionFormComponent
  ],
  imports: [
    CommonModule,
    JobDefinitionRoutingModule,
    MatButtonModule,
    MatExpansionModule,
    MatListModule,
    FlexLayoutModule,
    MatIconModule,
    ReactiveFormsModule,
    FormsModule,
    MatCheckboxModule,
    MatInputModule,
    MatDialogModule,
    MatProgressBarModule,
    MatSelectModule,
    DragDropModule,
    SharedModule
  ],
  entryComponents: [
    JobDefinitionNewDialogComponent,
    JobDefinitionInfoDialogComponent,
    JobTaskDefinitionNewDialogComponent,
    JobTaskDefinitionInfoDialogComponent
  ]
})
export class JobDefinitionModule { }
