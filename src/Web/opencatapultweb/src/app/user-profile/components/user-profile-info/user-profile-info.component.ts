import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/core/auth/auth.service';
import { FormBuilder } from '@angular/forms';
import { AccountService, UserDto, ManagedFileService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { reject } from 'q';

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
  updatedAvatar: File;

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

        if (data.avatarFileId) {
          this.avatar = this.managedFileService.getImageUrl(data.avatarFileId);
        }
      });
  }

  onSubmit() {
    if (this.userInfoForm.valid) {
      this.loading = true;
      const avatarPromise = new Promise((resolve) => {
        if (this.updatedAvatar) {
          if (this.user.avatarFileId) {
            this.managedFileService.updateManagedFile(this.user.avatarFileId, this.updatedAvatar).subscribe();
            resolve(this.user.avatarFileId);
          } else {
            this.managedFileService.createManagedFile(this.updatedAvatar)
              .subscribe((data) => resolve(data.id),
                (err) => {
                  reject(err);
                  this.snackbar.open(err);
                  this.loading = false;
                });
          }
        } else {
          resolve(null);
        }
      });

      avatarPromise.then((avatarFileId) => {
        this.accountService.updateUser(this.user.id,
          {
            id: this.user.id,
            avatarFileId: avatarFileId,
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
        this.avatar = fileReader.result;
      };

      fileReader.readAsDataURL(event.target.files[0]);

      this.updatedAvatar = event.target.files[0];
    }
  }
}
