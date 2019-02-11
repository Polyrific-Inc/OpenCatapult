import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobQueueRoutingModule } from './job-queue-routing.module';
import { JobQueueComponent } from './job-queue/job-queue.component';

@NgModule({
  declarations: [JobQueueComponent],
  imports: [
    CommonModule,
    JobQueueRoutingModule
  ]
})
export class JobQueueModule { }
