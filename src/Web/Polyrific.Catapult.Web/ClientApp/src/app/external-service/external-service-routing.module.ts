import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ExternalServiceComponent } from './external-service/external-service.component';

const routes: Routes = [
  {
    path: '',
    component: ExternalServiceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExternalServiceRoutingModule { }
