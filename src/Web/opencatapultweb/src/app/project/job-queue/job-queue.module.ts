import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobQueueRoutingModule } from './job-queue-routing.module';
import { JobQueueComponent } from './job-queue/job-queue.component';
import { JobQueueListComponent } from './job-queue-list/job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule,
  MatButtonModule, MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule, MatChipsModule, MatDividerModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { JobQueueDetailComponent } from './job-queue-detail/job-queue-detail.component';
import { JobQueueLogComponent } from './job-queue-log/job-queue-log.component';

@NgModule({
  declarations: [JobQueueComponent, JobQueueListComponent, JobQueueDetailComponent, JobQueueLogComponent],
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
    MatDividerModule
  ]
})
export class JobQueueModule { }
