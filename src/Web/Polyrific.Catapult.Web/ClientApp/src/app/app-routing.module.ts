import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { AuthGuard } from './core/auth/auth.guard';
import { AuthorizePolicy } from './core/auth/authorize-policy';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { LoginComponent } from './login/login.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { VersionComponent } from './version/version.component';
import { LoginWithTwofaComponent } from './login-with-twofa/login-with-twofa.component';
import { LoginWithRecoveryCodeComponent } from './login-with-recovery-code/login-with-recovery-code.component';

const routes: Routes = [
  {
    path: '',
    component: HomeLayoutComponent,
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: '/project',
        pathMatch: 'full'
      },
      {path: 'project', loadChildren: './project/project.module#ProjectModule'},
      {path: 'service', loadChildren: './external-service/external-service.module#ExternalServiceModule'},
      {
        path: 'engine', loadChildren: './engine/engine.module#EngineModule',
        data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}
      },
      {
        path: 'provider', loadChildren: './task-provider/task-provider.module#TaskProviderModule',
        data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}
      },
      {
        path: 'user', loadChildren: './account/account.module#AccountModule',
        data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}
      },
      {
        path: 'user-profile', loadChildren: './user-profile/user-profile.module#UserProfileModule'
      },
      {
        path: 'unauthorized',
        component: UnauthorizedComponent
      },
      {
        path: 'version',
        component: VersionComponent
      },
      {
        path: 'setting',
        loadChildren: './setting/setting.module#SettingModule'
      }
    ]
  },
  {
    path: 'login',
    component: LoginLayoutComponent,
    children: [
      {
        path: '',
        component: LoginComponent
      }
    ]
  },
  {
    path: 'login-with-2fa',
    component: LoginLayoutComponent,
    children: [
      {
        path: '',
        component: LoginWithTwofaComponent
      }
    ]
  },
  {
    path: 'login-with-recovery-code',
    component: LoginLayoutComponent,
    children: [
      {
        path: '',
        component: LoginWithRecoveryCodeComponent
      }
    ]
  },
  {
    path: 'forgot-password',
    component: ForgotPasswordComponent
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent
  },
  {
    path: 'confirm-email',
    component: ConfirmEmailComponent
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
