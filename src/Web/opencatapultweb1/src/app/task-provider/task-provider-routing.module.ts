import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TaskProviderComponent } from './task-provider/task-provider.component';

const routes: Routes = [
  {
    path: '',
    component: TaskProviderComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TaskProviderRoutingModule { }
