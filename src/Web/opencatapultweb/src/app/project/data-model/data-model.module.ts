import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DataModelRoutingModule } from './data-model-routing.module';
import { DataModelComponent } from './data-model/data-model.component';
import { MatButtonModule, MatExpansionModule, MatListModule, MatIconModule } from '@angular/material';
import { DataModelPropertyComponent } from './data-model-property/data-model-property.component';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [DataModelComponent, DataModelPropertyComponent],
  imports: [
    CommonModule,
    DataModelRoutingModule,
    MatButtonModule,
    MatExpansionModule,
    MatListModule,
    FlexLayoutModule,
    MatIconModule
  ]
})
export class DataModelModule { }
