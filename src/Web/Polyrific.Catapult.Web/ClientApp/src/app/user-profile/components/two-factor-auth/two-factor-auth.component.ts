import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';

@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrls: ['./two-factor-auth.component.css']
})
export class TwoFactorAuthComponent implements OnInit {
  form: FormGroup;
  sharedKey: string;
  authenticatorUri: string;

  constructor(
    private accountService: AccountService,
    private fb: FormBuilder,
    private snackbar: SnackbarService) { }

  ngOnInit() {
    this.form = this.fb.group({
      verificationCode: [null, Validators.required]
    });

    this.accountService.getTwoFactorAuthKey()
      .subscribe(data => {
        this.sharedKey = data.sharedKey;
        this.authenticatorUri = data.authenticatorUri;
      });
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onSubmit() {
    if (this.form.valid) {
      this.accountService.verifyTwoFactorAuthenticatorCode(this.form.value)
        .subscribe(() => {
          this.snackbar.open('verified');
        }, (err) => {
          this.snackbar.open(err);
        });
    }
  }

}
