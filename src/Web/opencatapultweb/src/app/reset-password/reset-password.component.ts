import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormControl, FormGroupDirective, NgForm, FormGroup } from '@angular/forms';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { ErrorStateMatcher } from '@angular/material';
import { ActivatedRoute } from '@angular/router';

class PasswordErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const invalidCtrl = !!(control && control.invalid && control.parent.dirty && control.touched);
    const invalidParent = !!(control && control.parent && control.parent.hasError('notSame') &&
      control.parent.dirty && (control.touched || form.submitted));

    return (invalidCtrl || invalidParent);
  }
}

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  loading = false;
  resetPasswordDone: boolean;
  email: string;
  token: string;
  matcher = new PasswordErrorStateMatcher();

  resetPasswordForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private route: ActivatedRoute,
    private snackbar: SnackbarService) {
    }

  ngOnInit() {
    this.email = this.route.snapshot.queryParams['email'];
    this.token = this.route.snapshot.queryParams['token'];

    this.resetPasswordForm = this.fb.group({
      token: [this.token, Validators.required],
      newPassword: [null, Validators.compose([Validators.required, Validators.minLength(6)])],
      confirmNewPassword: null
    }, {validators: this.checkPasswords});
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.resetPasswordForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onSubmit() {
    if (this.resetPasswordForm.valid) {
      this.loading = true;
      this.accountService.resetPassword(this.email, this.resetPasswordForm.value)
        .subscribe(
            data => {
              this.resetPasswordDone = true;
              this.loading = false;
            },
            (err) => {
                this.snackbar.open(err);
                this.loading = false;
            });
    }
  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    const pass = group.controls.newPassword.value;
    const confirmPass = group.controls.confirmNewPassword.value;

    return pass === confirmPass ? null : { notSame: true };
  }

  isPasswordMatchInvalid() {
    return this.resetPasswordForm.hasError('notSame');
  }
}
