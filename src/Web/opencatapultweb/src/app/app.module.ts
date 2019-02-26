import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule, MatButtonModule, MatIconModule, MatListModule,
          MatCardModule, MatInputModule, MatProgressBarModule, MatTableModule, MatPaginatorModule, MatSortModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeLayoutComponent,
    LoginLayoutComponent,
    LoginComponent,
    UnauthorizedComponent
  ],
  imports: [
    CoreModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    LayoutModule,
    MatProgressBarModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatCardModule,
    MatInputModule,
    ReactiveFormsModule,
    SharedModule.forRoot()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
