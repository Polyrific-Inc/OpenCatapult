import { Component, OnInit, Inject } from '@angular/core';
import { AccountService, ManagedFileService, UserDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthService } from '@app/core/auth/auth.service';

export interface AvatarDialogData {
  file: File;
  user: UserDto;
}

@Component({
  selector: 'app-avatar-dialog',
  templateUrl: './avatar-dialog.component.html',
  styleUrls: ['./avatar-dialog.component.css']
})
export class AvatarDialogComponent implements OnInit {
  loading: boolean;
  avatar: any;

  constructor (
    private accountService: AccountService,
    private authService: AuthService,
    private managedFileService: ManagedFileService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<AvatarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AvatarDialogData
    ) {
    }

  ngOnInit() {
    const fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.avatar = fileReader.result;
    };

    fileReader.readAsDataURL(this.data.file);
  }

  onSave() {
    this.loading = true;
      const avatarPromise = new Promise((resolve, reject) => {
        if (this.data.user.avatarFileId) {
          this.managedFileService.updateManagedFile(this.data.user.avatarFileId, this.data.file).subscribe();
          resolve(this.data.user.avatarFileId);
        } else {
          this.managedFileService.createManagedFile(this.data.file)
            .subscribe((data) => resolve(data.id),
              (err) => {
                reject(err);
                this.snackbar.open(err);
                this.loading = false;
              });
        }
      });

      avatarPromise.then((avatarFileId: number) => {
        this.accountService.updateAvatar(this.data.user.id, avatarFileId)
          .subscribe(
              () => {
                this.authService.refreshSession().subscribe();
                this.loading = false;
                this.snackbar.open('Avatar has been updated');
                this.dialogRef.close(avatarFileId);
              },
              err => {
                this.snackbar.open(err);
                this.loading = false;
              });
      });
  }
}
