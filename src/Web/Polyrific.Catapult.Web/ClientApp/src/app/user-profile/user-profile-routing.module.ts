import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileInfoComponent } from './components/user-profile-info/user-profile-info.component';
import { UserProfilePasswordComponent } from './components/user-profile-password/user-profile-password.component';
import { TwoFactorAuthComponent } from './components/two-factor-auth/two-factor-auth.component';

const routes: Routes = [
  {
    path: '',
    component: UserProfileComponent,
    children: [
      {path: '', redirectTo: 'info', pathMatch: 'full'},
      {path: 'info', component: UserProfileInfoComponent},
      {path: 'password', component: UserProfilePasswordComponent},
      {path: 'twofactor', component: TwoFactorAuthComponent},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserProfileRoutingModule { }
