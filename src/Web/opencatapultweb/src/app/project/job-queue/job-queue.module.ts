import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobQueueRoutingModule } from './job-queue-routing.module';
import { JobQueueComponent } from './job-queue/job-queue.component';
import { JobQueueListComponent } from './job-queue-list/job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule,
  MatButtonModule, MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule,
  MatSortModule, MatChipsModule, MatDividerModule, MatListModule, MatInputModule,
  MatProgressBarModule, MatDialogModule, MatExpansionModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { JobQueueDetailComponent } from './job-queue-detail/job-queue-detail.component';
import { JobQueueLogComponent } from './job-queue-log/job-queue-log.component';
import { JobQueueStatusComponent } from './components/job-queue-status/job-queue-status.component';
import { SharedModule } from '@app/shared/shared.module';
import { JobQueueCancelDialogComponent } from './components/job-queue-cancel-dialog/job-queue-cancel-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { JobQueueTaskStatusComponent } from './components/job-queue-task-status/job-queue-task-status.component';
import { JobQueueTaskLogComponent } from './job-queue-task-log/job-queue-task-log.component';

@NgModule({
  declarations: [
    JobQueueComponent,
    JobQueueListComponent,
    JobQueueDetailComponent,
    JobQueueLogComponent,
    JobQueueStatusComponent,
    JobQueueCancelDialogComponent,
    JobQueueTaskStatusComponent,
    JobQueueTaskLogComponent
  ],
  imports: [
    CommonModule,
    JobQueueRoutingModule,
    MatTabsModule,
    MatIconModule,
    MatBadgeModule,
    MatTableModule,
    MatButtonModule,
    MatTooltipModule,
    MatProgressSpinnerModule,
    FlexLayoutModule,
    MatPaginatorModule,
    MatSortModule,
    MatChipsModule,
    MatDividerModule,
    MatListModule,
    SharedModule,
    ReactiveFormsModule,
    MatInputModule,
    MatProgressBarModule,
    MatDialogModule,
    MatExpansionModule
  ],
  entryComponents: [
    JobQueueCancelDialogComponent
  ]
})
export class JobQueueModule { }
