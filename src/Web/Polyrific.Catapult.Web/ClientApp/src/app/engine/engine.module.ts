import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EngineRoutingModule } from './engine-routing.module';
import { EngineComponent } from './engine/engine.component';
import { MatTableModule, MatButtonModule, MatDialogModule, MatIconModule,
  MatSelectModule, MatTooltipModule, MatProgressBarModule, MatInputModule,
  MatDatepickerModule, MatNativeDateModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { FlexModule } from '@angular/flex-layout';
import { EngineTokenDialogComponent } from './components/engine-token-dialog/engine-token-dialog.component';
import { EngineRegisterDialogComponent } from './components/engine-register-dialog/engine-register-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [EngineComponent, EngineTokenDialogComponent, EngineRegisterDialogComponent],
  imports: [
    CommonModule,
    EngineRoutingModule,
    MatTableModule,
    SharedModule,
    MatButtonModule,
    MatDialogModule,
    FlexModule,
    MatIconModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatTooltipModule,
    MatProgressBarModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  entryComponents: [
    EngineTokenDialogComponent,
    EngineRegisterDialogComponent
  ]
})
export class EngineModule { }
