import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExternalServiceRoutingModule } from './external-service-routing.module';
import { ExternalServiceComponent } from './external-service/external-service.component';
import { MatTableModule, MatIconModule, MatButtonModule, MatDialogModule,
  MatInputModule, MatSelectModule, MatProgressBarModule, MatDividerModule, MatProgressSpinnerModule } from '@angular/material';
import { ExternalServiceInfoDialogComponent } from './components/external-service-info-dialog/external-service-info-dialog.component';
import { ExternalServiceNewDialogComponent } from './components/external-service-new-dialog/external-service-new-dialog.component';
import { ExternalServiceFormComponent } from './components/external-service-form/external-service-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ExternalServicePropertyFormComponent } from './components/external-service-property-form/external-service-property-form.component';
import { FlexModule } from '@angular/flex-layout';
import { ExternalServiceGenericFormComponent } from './components/external-service-generic-form/external-service-generic-form.component';
import { GenericService } from './services/generic.service';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    ExternalServiceComponent,
    ExternalServiceInfoDialogComponent,
    ExternalServiceNewDialogComponent,
    ExternalServiceFormComponent,
    ExternalServicePropertyFormComponent,
    ExternalServiceGenericFormComponent
  ],
  imports: [
    CommonModule,
    FlexModule,
    ExternalServiceRoutingModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    SharedModule
  ],
  providers: [
    GenericService
  ],
  entryComponents: [ExternalServiceInfoDialogComponent, ExternalServiceNewDialogComponent]
})
export class ExternalServiceModule { }
