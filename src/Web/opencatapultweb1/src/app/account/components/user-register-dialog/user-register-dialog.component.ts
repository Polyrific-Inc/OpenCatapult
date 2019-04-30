import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AccountService, UserDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-user-register-dialog',
  templateUrl: './user-register-dialog.component.html',
  styleUrls: ['./user-register-dialog.component.css']
})
export class UserRegisterDialogComponent implements OnInit {
  userForm: FormGroup;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<UserRegisterDialogComponent>
    ) {
    }

  ngOnInit() {
    this.userForm = this.fb.group({
      email: [null, Validators.compose([Validators.required, Validators.email])],
      firstName: null,
      lastName: null
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.loading = true;
      this.accountService.register(this.userForm.value)
        .subscribe(
            (data: UserDto) => {
              this.loading = false;
              this.snackbar.open('New user has been registered');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.userForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
