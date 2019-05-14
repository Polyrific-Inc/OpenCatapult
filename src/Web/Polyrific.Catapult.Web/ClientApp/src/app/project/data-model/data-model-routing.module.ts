import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataModelComponent } from './data-model/data-model.component';

const routes: Routes = [
  {
    path: '', component: DataModelComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DataModelRoutingModule { }
