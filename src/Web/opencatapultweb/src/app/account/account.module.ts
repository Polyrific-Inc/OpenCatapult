import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { AccountComponent } from './account/account.component';
import { MatTableModule, MatButtonModule, MatIconModule, MatSelectModule,
  MatTooltipModule, MatDialogModule, MatProgressBarModule, MatInputModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@app/shared/shared.module';
import { UserSetRoleDialogComponent } from './components/user-set-role-dialog/user-set-role-dialog.component';
import { UserInfoDialogComponent } from './components/user-info-dialog/user-info-dialog.component';
import { UserRegisterDialogComponent } from './components/user-register-dialog/user-register-dialog.component';

@NgModule({
  declarations: [AccountComponent, UserSetRoleDialogComponent, UserInfoDialogComponent, UserRegisterDialogComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    FlexLayoutModule,
    ReactiveFormsModule,
    MatTooltipModule,
    SharedModule,
    MatInputModule,
    MatProgressBarModule,
    MatDialogModule
  ],
  entryComponents: [
    UserSetRoleDialogComponent,
    UserInfoDialogComponent,
    UserRegisterDialogComponent
  ]
})
export class AccountModule { }
