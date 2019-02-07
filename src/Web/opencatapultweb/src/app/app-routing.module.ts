import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { LoginComponent } from './auth/login/login.component';

const routes: Routes = [
  {
    path: '',
    component: HomeLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: '/project',
        pathMatch: 'full'
      },
      {path: 'project', loadChildren: './project/project.module#ProjectModule'},
      {path: 'service', loadChildren: './external-service/external-service.module#ExternalServiceModule'},
      {path: 'engine', loadChildren: './engine/engine.module#EngineModule'},
      {path: 'provider', loadChildren: './task-provider/task-provider.module#TaskProviderModule'},
      {path: 'account', loadChildren: './account/account.module#AccountModule'}
    ]
  },
  {
    path: '',
    component: LoginLayoutComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent
      }
    ]
  }
];

// const routes: Routes = [
//   {path: '', redirectTo: '/project'},
//   {path: 'project', loadChildren: './project/project.module#ProjectModule'}
// ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
