import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';
import { ProjectNewComponent } from './project-new/project-new.component';
import { ProjectCloneComponent } from './project-clone/project-clone.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent,
    children:
    [
      {
        path: 'new',
        component: ProjectNewComponent
      },
      {
        path: ':id/clone',
        component: ProjectCloneComponent
      },
      {
        path: ':id',
        component: ProjectDetailComponent,
        children: [
          {path: '', redirectTo: 'info', pathMatch: 'full'},
          {path: 'info', component: ProjectInfoComponent },
          {path: 'data-model', loadChildren: './data-model/data-model.module#DataModelModule' },
          {path: 'job-definition', loadChildren: './job-definition/job-definition.module#JobDefinitionModule'},
          {path: 'job-queue', loadChildren: './job-queue/job-queue.module#JobQueueModule'},
          {path: 'member', loadChildren: './member/member.module#MemberModule'}
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule { }
