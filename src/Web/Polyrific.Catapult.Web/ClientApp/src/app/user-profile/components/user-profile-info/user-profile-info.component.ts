import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { AuthService } from '@app/core/auth/auth.service';
import { FormBuilder, FormControl } from '@angular/forms';
import { AccountService, UserDto, ManagedFileService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialog } from '@angular/material';
import { AvatarDialogComponent } from '../avatar-dialog/avatar-dialog.component';

@Component({
  selector: 'app-user-profile-info',
  templateUrl: './user-profile-info.component.html',
  styleUrls: ['./user-profile-info.component.css']
})
export class UserProfileInfoComponent implements OnInit {
  @ViewChild('avatarControl') avatarControlVariable: ElementRef;

  userInfoForm = this.fb.group({
    id: [{value: null, disabled: true}],
    userName: [{value: null, disabled: true}],
    firstName: [{value: null, disabled: true}],
    lastName: [{value: null, disabled: true}],
  });
  user: UserDto;
  editing: boolean;
  loading: boolean;
  avatar: any;
  updatedAvatar: File;
  avatarControl = new FormControl();

  constructor (
    private fb: FormBuilder,
    private dialog: MatDialog,
    private accountService: AccountService,
    private authService: AuthService,
    private snackbar: SnackbarService,
    private managedFileService: ManagedFileService
    ) {
    }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.loading = true;
    this.accountService.getUserByUserName(this.authService.currentUserValue.userName)
      .subscribe(data => {
        this.loading = false;
        this.user = data;
        this.userInfoForm.patchValue(data);

        if (data.avatarFileId) {
          this.avatar = this.managedFileService.getImageUrl(data.avatarFileId);
        }
      });
  }

  onSubmit() {
    if (this.userInfoForm.valid) {
      this.loading = true;
      this.accountService.updateUser(this.user.id,
        {
          id: this.user.id,
          ...this.userInfoForm.value
        })
        .subscribe(
            () => {
              this.authService.refreshSession().subscribe();
              this.loading = false;
              this.editing = false;
              this.user = {
                id: this.user.id,
                ...this.userInfoForm.value
              };
              this.userInfoForm.get('userName').disable();
              this.userInfoForm.get('firstName').disable();
              this.userInfoForm.get('lastName').disable();
              this.snackbar.open('User info has been updated');
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.userInfoForm.get('userName').enable();
    this.userInfoForm.get('firstName').enable();
    this.userInfoForm.get('lastName').enable();
    this.editing = true;
  }

  onCancelClick() {
    this.userInfoForm.get('userName').disable();
    this.userInfoForm.get('firstName').disable();
    this.userInfoForm.get('lastName').disable();
    this.editing = false;
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.userInfoForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onAvatarChanged(event) {
    if (event.target.value) {
      const dialogRef = this.dialog.open(AvatarDialogComponent, {
        data: {
          user: this.user,
          file: event.target.files[0]
        }
      });

      dialogRef.afterClosed().subscribe(avatarFileId => {
        if (avatarFileId) {
          this.avatar = this.managedFileService.getImageUrl(avatarFileId);
        }

        this.avatarControlVariable.nativeElement.value = '';
      });
    }
  }
}
