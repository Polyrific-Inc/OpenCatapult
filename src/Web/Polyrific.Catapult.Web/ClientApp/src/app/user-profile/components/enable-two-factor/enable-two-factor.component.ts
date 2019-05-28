import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { Router } from '@angular/router';

@Component({
  selector: 'app-enable-two-factor',
  templateUrl: './enable-two-factor.component.html',
  styleUrls: ['./enable-two-factor.component.css']
})
export class EnableTwoFactorComponent implements OnInit {
  loading: boolean;
  recoveryCodesCount: number;
  form: FormGroup;
  sharedKey: string;
  authenticatorUri: string;
  recoveryCodes: string[];

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private snackbar: SnackbarService,
    private router: Router) { }

  ngOnInit() {
    this.form = this.fb.group({
      verificationCode: [null, Validators.required]
    });

    this.loading = true;
    this.accountService.getUser2faInfo()
      .subscribe(user2faInfo => {
        this.recoveryCodesCount = user2faInfo.recoveryCodesLeft;

        this.accountService.getTwoFactorAuthKey()
          .subscribe(data => {
            this.loading = false;
            this.sharedKey = data.sharedKey;
            this.authenticatorUri = data.authenticatorUri;
          });
      });
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onSubmit() {
    if (this.form.valid) {
      this.loading = true;
      this.accountService.verifyTwoFactorAuthenticatorCode(this.form.value)
        .subscribe(() => {
          this.snackbar.open('Your authenticator app has been verified.');

          if (this.recoveryCodesCount === 0) {
            this.accountService.generate2faRecoveryCodes()
              .subscribe(data => {
                this.loading = false;
                this.recoveryCodes = data.recoveryCodes;
              });
          } else {
            this.router.navigate(['/user-profile/twofactor']);
          }
        }, (err) => {
          this.snackbar.open(err);
        });
    }
  }

}
