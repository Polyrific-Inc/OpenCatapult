import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule, MatButtonModule, MatIconModule, MatListModule,
          MatCardModule, MatInputModule, MatProgressBarModule, MatMenuModule, MatSidenavModule, MatDialogModule } from '@angular/material';
import { ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { HomeLayoutComponent } from './layouts/home-layout/home-layout.component';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

import { ConfigServiceProvider } from './config/config.service.provider';
import { HelpContextDialogComponent } from './help-context-dialog/help-context-dialog.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { FooterComponent } from './footer/footer.component';
import { VersionComponent } from './version/version.component';
import { LoginWithTwofaComponent } from './login-with-twofa/login-with-twofa.component';
import { LoginWithRecoveryCodeComponent } from './login-with-recovery-code/login-with-recovery-code.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeLayoutComponent,
    LoginLayoutComponent,
    LoginComponent,
    UnauthorizedComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    HelpContextDialogComponent,
    ConfirmEmailComponent,
    FooterComponent,
    VersionComponent,
    LoginWithTwofaComponent,
    LoginWithRecoveryCodeComponent
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
    MatMenuModule,
    MatSidenavModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatDialogModule,
    SharedModule.forRoot()
  ],
  providers: [
    ConfigServiceProvider
  ],
  bootstrap: [AppComponent],
  entryComponents: [
    HelpContextDialogComponent
  ]
})
export class AppModule { }
