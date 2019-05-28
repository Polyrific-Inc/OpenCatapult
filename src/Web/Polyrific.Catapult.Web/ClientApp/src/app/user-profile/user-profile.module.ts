import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserProfileRoutingModule } from './user-profile-routing.module';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileInfoComponent } from './components/user-profile-info/user-profile-info.component';
import { UserProfilePasswordComponent } from './components/user-profile-password/user-profile-password.component';
import { MatTabsModule, MatInputModule, MatProgressBarModule, MatButtonModule, MatIconModule, MatDialogModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AvatarDialogComponent } from './components/avatar-dialog/avatar-dialog.component';
import { SharedModule } from '@app/shared/shared.module';
import { TwoFactorAuthComponent } from './components/two-factor-auth/two-factor-auth.component';
import { QRCodeModule } from 'angularx-qrcode';
import { EnableTwoFactorComponent } from './components/enable-two-factor/enable-two-factor.component';
import { ResetTwoFactorComponent } from './components/reset-two-factor/reset-two-factor.component';
import { DisableTwoFactorComponent } from './components/disable-two-factor/disable-two-factor.component';
import { ResetRecoveryTwoFactorComponent } from './components/reset-recovery-two-factor/reset-recovery-two-factor.component';
import { ShowRecoveryTwoFactorComponent } from './components/show-recovery-two-factor/show-recovery-two-factor.component';

@NgModule({
  declarations: [
    UserProfileComponent,
    UserProfileInfoComponent,
    UserProfilePasswordComponent,
    AvatarDialogComponent,
    TwoFactorAuthComponent,
    EnableTwoFactorComponent,
    ResetTwoFactorComponent,
    DisableTwoFactorComponent,
    ResetRecoveryTwoFactorComponent,
    ShowRecoveryTwoFactorComponent
  ],
  imports: [
    CommonModule,
    UserProfileRoutingModule,
    MatTabsModule,
    MatInputModule,
    ReactiveFormsModule,
    MatProgressBarModule,
    MatButtonModule,
    FlexLayoutModule,
    MatIconModule,
    MatDialogModule,
    SharedModule,
    QRCodeModule
  ],
  entryComponents: [AvatarDialogComponent]
})
export class UserProfileModule { }
