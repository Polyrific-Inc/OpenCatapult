import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { LoginComponent } from './auth/login/login.component';
import { AuthorizePolicy } from './auth/authorize-policy';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

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
      {path: 'engine', loadChildren: './engine/engine.module#EngineModule', data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}},
      {path: 'provider', loadChildren: './task-provider/task-provider.module#TaskProviderModule', data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}},
      {path: 'user', loadChildren: './account/account.module#AccountModule', data: { authPolicy: AuthorizePolicy.UserRoleAdminAccess}},
      {
        path: 'unauthorized',
        component: UnauthorizedComponent
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
  }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
