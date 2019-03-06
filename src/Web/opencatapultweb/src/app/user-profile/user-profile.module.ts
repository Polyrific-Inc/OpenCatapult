import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserProfileRoutingModule } from './user-profile-routing.module';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserProfileInfoComponent } from './components/user-profile-info/user-profile-info.component';
import { UserProfilePasswordComponent } from './components/user-profile-password/user-profile-password.component';
import { MatTabsModule, MatInputModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [UserProfileComponent, UserProfileInfoComponent, UserProfilePasswordComponent],
  imports: [
    CommonModule,
    UserProfileRoutingModule,
    MatTabsModule,
    MatInputModule,
    ReactiveFormsModule,
    MatProgressBarModule,
    MatButtonModule,
    FlexLayoutModule
  ]
})
export class UserProfileModule { }
