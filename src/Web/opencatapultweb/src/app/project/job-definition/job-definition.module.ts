import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { JobDefinitionRoutingModule } from './job-definition-routing.module';
import { JobDefinitionComponent } from './job-definition/job-definition.component';

@NgModule({
  declarations: [JobDefinitionComponent],
  imports: [
    CommonModule,
    JobDefinitionRoutingModule
  ]
})
export class JobDefinitionModule { }
