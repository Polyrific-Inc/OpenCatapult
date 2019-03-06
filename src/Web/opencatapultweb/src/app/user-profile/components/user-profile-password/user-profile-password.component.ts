import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, FormGroup, FormControl, FormGroupDirective, NgForm } from '@angular/forms';
import { UserDto, AccountService } from '@app/core';
import { AuthService } from '@app/core/auth/auth.service';
import { SnackbarService } from '@app/shared';
import { ErrorStateMatcher } from '@angular/material';

class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const invalidCtrl = !!(control && control.invalid && control.parent.dirty && control.touched);
    const invalidParent = !!(control && control.parent && control.parent.hasError('notSame') &&
      control.parent.dirty && (control.touched || form.submitted));

    return (invalidCtrl || invalidParent);
  }
}

@Component({
  selector: 'app-user-profile-password',
  templateUrl: './user-profile-password.component.html',
  styleUrls: ['./user-profile-password.component.css']
})
export class UserProfilePasswordComponent implements OnInit {
  userPasswordForm = this.fb.group({
    oldPassword: [null, Validators.required],
    newPassword: [null, Validators.compose([Validators.required, Validators.minLength(6)])],
    confirmNewPassword: null
  }, {validators: this.checkPasswords});
  matcher = new MyErrorStateMatcher();
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    ) {
    }

  ngOnInit() {
  }

  onSubmit(formDirective: FormGroupDirective) {
    if (this.userPasswordForm.valid) {
      this.loading = true;
      this.accountService.updatePassword(this.userPasswordForm.value)
        .subscribe(
            () => {
              this.loading = false;
              formDirective.resetForm();
              this.userPasswordForm.reset();
              this.snackbar.open('User password has been updated');
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.userPasswordForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  isPasswordMatchInvalid() {
    return this.userPasswordForm.hasError('notSame');
  }

  checkPasswords(group: FormGroup) { // here we have the 'passwords' group
    const pass = group.controls.newPassword.value;
    const confirmPass = group.controls.confirmNewPassword.value;

    return pass === confirmPass ? null : { notSame: true };
  }
}
