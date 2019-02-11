import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DataModelRoutingModule } from './data-model-routing.module';
import { DataModelComponent } from './data-model/data-model.component';

@NgModule({
  declarations: [DataModelComponent],
  imports: [
    CommonModule,
    DataModelRoutingModule
  ]
})
export class DataModelModule { }
