import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/core/auth/auth.service';
import { FormBuilder } from '@angular/forms';
import { AccountService, UserDto, ManagedFileService } from '@app/core';
import { SnackbarService } from '@app/shared';

@Component({
  selector: 'app-user-profile-info',
  templateUrl: './user-profile-info.component.html',
  styleUrls: ['./user-profile-info.component.css']
})
export class UserProfileInfoComponent implements OnInit {
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
  updateAvatarFileName: string;
  updatedAvatar: any;

  constructor (
    private fb: FormBuilder,
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
    this.accountService.getUserByEmail(this.authService.currentUserValue.email)
      .subscribe(data => {
        this.loading = false;
        this.user = data;
        this.userInfoForm.patchValue(data);
        this.avatar = this.managedFileService.getImagePath(data.avatarFile);
      });
  }

  onSubmit() {
    if (this.userInfoForm.valid) {
      this.loading = true;
      this.accountService.updateUser(this.user.id,
        {
          id: this.user.id,
          avatarFile: this.updatedAvatar ? {
            id: this.user.avatarFile ? this.user.avatarFile.id : 0,
            fileName: this.updateAvatarFileName,
            file: this.updatedAvatar
          } : null,
          ...this.userInfoForm.value
        })
        .subscribe(
            () => {
              this.authService.refreshSession().subscribe();
              this.loading = false;
              this.editing = false;
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
    this.userInfoForm.get('firstName').enable();
    this.userInfoForm.get('lastName').enable();
    this.editing = true;
  }

  onCancelClick() {
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
      this.updateAvatarFileName = event.target.value.split(/(\\|\/)/g).pop();

      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        // @ts-ignore
        this.updatedAvatar = fileReader.result.split(',')[1];
        this.avatar = fileReader.result;
      };
      fileReader.readAsDataURL(event.target.files[0]);
    }
  }
}
