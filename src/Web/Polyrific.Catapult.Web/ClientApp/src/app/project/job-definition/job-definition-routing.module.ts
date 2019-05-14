import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JobDefinitionComponent } from './job-definition/job-definition.component';

const routes: Routes = [
  {path: '', component: JobDefinitionComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobDefinitionRoutingModule { }
