import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExternalServiceRoutingModule } from './external-service-routing.module';
import { ExternalServiceComponent } from './external-service/external-service.component';

@NgModule({
  declarations: [ExternalServiceComponent],
  imports: [
    CommonModule,
    ExternalServiceRoutingModule
  ]
})
export class ExternalServiceModule { }
