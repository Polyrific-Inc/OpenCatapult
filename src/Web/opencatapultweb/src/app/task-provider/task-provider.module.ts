import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskProviderRoutingModule } from './task-provider-routing.module';
import { TaskProviderComponent } from './task-provider/task-provider.component';

@NgModule({
  declarations: [TaskProviderComponent],
  imports: [
    CommonModule,
    TaskProviderRoutingModule
  ]
})
export class TaskProviderModule { }
