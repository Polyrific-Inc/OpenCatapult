import { Component, OnInit, Inject } from '@angular/core';
import { AccountService, UserDto } from '@app/core';
import { FormBuilder } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-user-info-dialog',
  templateUrl: './user-info-dialog.component.html',
  styleUrls: ['./user-info-dialog.component.css']
})
export class UserInfoDialogComponent implements OnInit {
  userInfoForm = this.fb.group({
    id: [{value: null, disabled: true}],
    userName: [{value: null, disabled: true}],
    firstName: [{value: null, disabled: true}],
    lastName: [{value: null, disabled: true}],
  });
  editing: boolean;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<UserInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public user: UserDto
    ) {
    }

  ngOnInit() {
    this.userInfoForm.patchValue(this.user);
  }

  onSubmit() {
    if (this.userInfoForm.valid) {
      this.loading = true;
      this.accountService.updateUser(this.user.id,
        { id: this.user.id, ...this.userInfoForm.value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('User has been updated');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.userInfoForm.get('firstName').enable();
    this.userInfoForm.get('lastName').enable();
    this.editing = true;
  }

  onCancelClick() {
    this.userInfoForm.get('firstName').disable();
    this.userInfoForm.get('lastName').enable();
    this.editing = false;
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.userInfoForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }
}
