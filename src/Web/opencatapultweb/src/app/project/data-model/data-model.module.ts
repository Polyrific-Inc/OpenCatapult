import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DataModelRoutingModule } from './data-model-routing.module';
import { DataModelComponent } from './data-model/data-model.component';
import { MatButtonModule, MatExpansionModule, MatListModule, MatIconModule,
  MatInputModule, MatCheckboxModule, MatDialogModule, MatProgressBarModule } from '@angular/material';
import { DataModelPropertyComponent } from './data-model-property/data-model-property.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { DataModelFormComponent } from './components/data-model-form/data-model-form.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { DataModelNewDialogComponent } from './components/data-model-new-dialog/data-model-new-dialog.component';
import { DataModelInfoDialogComponent } from './components/data-model-info-dialog/data-model-info-dialog.component';
import { DataModelPropertyFormComponent } from './components/data-model-property-form/data-model-property-form.component';

@NgModule({
  declarations: [
    DataModelComponent,
    DataModelPropertyComponent,
    DataModelFormComponent,
    DataModelNewDialogComponent,
    DataModelInfoDialogComponent,
    DataModelPropertyFormComponent
  ],
  imports: [
    CommonModule,
    DataModelRoutingModule,
    MatButtonModule,
    MatExpansionModule,
    MatListModule,
    FlexLayoutModule,
    MatIconModule,
    ReactiveFormsModule,
    FormsModule,
    MatCheckboxModule,
    MatInputModule,
    MatDialogModule,
    MatProgressBarModule
  ],
  entryComponents: [
    DataModelNewDialogComponent,
    DataModelInfoDialogComponent
  ]
})
export class DataModelModule { }
