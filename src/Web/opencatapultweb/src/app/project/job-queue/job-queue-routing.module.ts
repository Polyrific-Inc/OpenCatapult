import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JobQueueComponent } from './job-queue/job-queue.component';
import { JobQueueDetailComponent } from './job-queue-detail/job-queue-detail.component';
import { JobQueueLogComponent } from './job-queue-log/job-queue-log.component';

const routes: Routes = [
  {
    path: '', component: JobQueueComponent
  },
  {
    path: ':id', component: JobQueueDetailComponent
  },
  {
    path: ':id/logs', component: JobQueueLogComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobQueueRoutingModule { }
