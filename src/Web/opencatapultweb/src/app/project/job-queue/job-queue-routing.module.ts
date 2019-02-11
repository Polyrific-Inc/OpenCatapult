import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JobQueueComponent } from './job-queue/job-queue.component';

const routes: Routes = [
  {
    path: '', component: JobQueueComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobQueueRoutingModule { }
