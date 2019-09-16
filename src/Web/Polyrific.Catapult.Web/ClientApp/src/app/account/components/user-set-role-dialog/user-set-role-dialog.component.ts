import { Component, OnInit, Inject } from '@angular/core';
import { userRoles, UserDto, AccountService } from '@app/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-user-set-role-dialog',
  templateUrl: './user-set-role-dialog.component.html',
  styleUrls: ['./user-set-role-dialog.component.css']
})
export class UserSetRoleDialogComponent implements OnInit {
  userRoleForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}],
    userName: [{value: null, disabled: true}],
    roleName: [{value: null, disabled: true}, Validators.required],
  });
  editing: boolean;
  loading: boolean;
  userRoles = userRoles;

  constructor (
    private fb: FormBuilder,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<UserSetRoleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public user: UserDto
    ) {
    }

  ngOnInit() {
    this.userRoleForm.patchValue({
      id : this.user.id,
      userName : this.user.userName,
      roleName : this.user.role
    });
  }

  onSubmit() {
    if (this.userRoleForm.valid) {
      this.loading = true;
      this.accountService.setUserRole(this.user.id,
        { userId: this.user.id, roleName: this.userRoleForm.get('roleName').value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('User role has been updated');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.userRoleForm.get('roleName').enable();
    this.editing = true;
  }

  onCancelClick() {
    this.userRoleForm.get('roleName').disable();
    this.editing = false;
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.userRoleForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
