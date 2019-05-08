import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  loading = false;
  resetPasswordRequested: boolean;

  forgotPasswordForm = this.fb.group({
    email: [null, Validators.compose([
      Validators.required, Validators.email])]
  });

  private formSubmitAttempt: boolean;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackbar: SnackbarService) {
    }

  ngOnInit() {
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.forgotPasswordForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onSubmit() {
    if (this.forgotPasswordForm.valid) {
        this.loading = true;
      this.accountService.requestResetPassword(this.forgotPasswordForm.value.email)
        .subscribe(
            data => {
              this.resetPasswordRequested = true;
              this.loading = false;
            },
            (err) => {
                this.snackbar.open(err);
                this.loading = false;
            });
    }

    this.formSubmitAttempt = true;
  }
}
