import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingComponent } from './setting/setting.component';
import { SettingFieldComponent } from './components/setting-field/setting-field.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule, MatSelectModule, MatCheckboxModule, MatButtonModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { SettingRoutingModule } from './setting-routing.module';

@NgModule({
  declarations: [SettingComponent, SettingFieldComponent],
  imports: [
    CommonModule,
    SettingRoutingModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
    SharedModule
  ]
})
export class SettingModule { }
