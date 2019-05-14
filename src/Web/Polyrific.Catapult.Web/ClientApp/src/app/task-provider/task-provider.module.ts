import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskProviderRoutingModule } from './task-provider-routing.module';
import { TaskProviderComponent } from './task-provider/task-provider.component';
import { MatTableModule, MatButtonModule, MatIconModule, MatDialogModule,
  MatSelectModule, MatInputModule, MatChipsModule, MatProgressBarModule, MatTooltipModule, MatAutocompleteModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { TaskProviderRegisterDialogComponent } from './components/task-provider-register-dialog/task-provider-register-dialog.component';
import { FlexModule } from '@angular/flex-layout';
import { TaskProviderInfoDialogComponent } from './components/task-provider-info-dialog/task-provider-info-dialog.component';

@NgModule({
  declarations: [TaskProviderComponent, TaskProviderRegisterDialogComponent, TaskProviderInfoDialogComponent],
  imports: [
    CommonModule,
    TaskProviderRoutingModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatInputModule,
    MatChipsModule,
    SharedModule,
    FlexModule,
    MatProgressBarModule,
    MatTooltipModule,
    MatAutocompleteModule
  ],
  entryComponents: [
    TaskProviderRegisterDialogComponent,
    TaskProviderInfoDialogComponent
  ]
})
export class TaskProviderModule { }
