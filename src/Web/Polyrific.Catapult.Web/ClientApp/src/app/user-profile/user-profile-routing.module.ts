import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileInfoComponent } from './components/user-profile-info/user-profile-info.component';
import { UserProfilePasswordComponent } from './components/user-profile-password/user-profile-password.component';
import { TwoFactorAuthComponent } from './components/two-factor-auth/two-factor-auth.component';
import { EnableTwoFactorComponent } from './components/enable-two-factor/enable-two-factor.component';
import { ResetTwoFactorComponent } from './components/reset-two-factor/reset-two-factor.component';
import { ResetRecoveryTwoFactorComponent } from './components/reset-recovery-two-factor/reset-recovery-two-factor.component';
import { DisableTwoFactorComponent } from './components/disable-two-factor/disable-two-factor.component';

const routes: Routes = [
  {
    path: '',
    component: UserProfileComponent,
    children: [
      {path: '', redirectTo: 'info', pathMatch: 'full'},
      {path: 'info', component: UserProfileInfoComponent},
      {path: 'password', component: UserProfilePasswordComponent},
      {
        path: 'twofactor',
        component: TwoFactorAuthComponent,
      },
      {path: 'twofactor/enable', component: EnableTwoFactorComponent},
      {path: 'twofactor/reset', component: ResetTwoFactorComponent},
      {path: 'twofactor/disable', component: DisableTwoFactorComponent},
      {path: 'twofactor/reset-recovery', component: ResetRecoveryTwoFactorComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserProfileRoutingModule { }
