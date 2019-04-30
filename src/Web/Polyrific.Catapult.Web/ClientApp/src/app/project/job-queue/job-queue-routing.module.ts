import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JobQueueComponent } from './job-queue/job-queue.component';
import { JobQueueDetailComponent } from './job-queue-detail/job-queue-detail.component';
import { JobQueueLogComponent } from './job-queue-log/job-queue-log.component';
import { JobQueueResolverService } from './services/job-queue-resolver.service';
import { JobQueueErrorComponent } from './job-queue-error/job-queue-error.component';
import { JobQueueListComponent } from './job-queue-list/job-queue-list.component';

const routes: Routes = [
  {
    path: 'list', component: JobQueueComponent,
    children: [
      {path: '', redirectTo: 'current', pathMatch: 'full'},
      {path: 'current', component: JobQueueListComponent},
      {path: 'pending', component: JobQueueListComponent},
      {path: 'past', component: JobQueueListComponent},
    ]
  },
  {path: '', redirectTo: 'list', pathMatch: 'full'},
  {
    path: ':id', component: JobQueueDetailComponent,
    resolve: {
      jobQueue: JobQueueResolverService
    }
  },
  {
    path: ':id/logs', component: JobQueueLogComponent,
    resolve: {
      jobQueue: JobQueueResolverService
    }
  },
  {
    path: ':id/error', component: JobQueueErrorComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobQueueRoutingModule { }
