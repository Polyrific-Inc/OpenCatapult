import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from './project/project.component';
import { ProjectDetailComponent } from './project-detail/project-detail.component';
import { ProjectInfoComponent } from './project-info/project-info.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent
  },
  {
    path: ':id',
    component: ProjectDetailComponent,
    children: [
      {path: '', component: ProjectInfoComponent },
      {path: 'data-model', loadChildren: './data-model/data-model.module#DataModelModule' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule { }
