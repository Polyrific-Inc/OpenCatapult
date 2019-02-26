import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobQueueRoutingModule } from './job-queue-routing.module';
import { JobQueueComponent } from './job-queue/job-queue.component';
import { JobQueueListComponent } from './job-queue-list/job-queue-list.component';
import { MatTabsModule, MatIconModule, MatBadgeModule, MatTableModule,
  MatButtonModule, MatTooltipModule, MatProgressSpinnerModule, MatPaginatorModule, MatSortModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [JobQueueComponent, JobQueueListComponent],
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
    MatSortModule
  ]
})
export class JobQueueModule { }
