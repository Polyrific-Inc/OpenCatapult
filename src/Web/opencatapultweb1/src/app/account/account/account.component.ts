import { Component, OnInit, AfterViewInit } from '@angular/core';
import { UserDto, AccountService } from '@app/core';
import { FormControl, FormBuilder } from '@angular/forms';
import { UserStatus } from '@app/core/enums/user-status';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { UserRole } from '@app/core/enums/user-role';
import { UserSetRoleDialogComponent } from '../components/user-set-role-dialog/user-set-role-dialog.component';
import { UserInfoDialogComponent } from '../components/user-info-dialog/user-info-dialog.component';
import { UserRegisterDialogComponent } from '../components/user-register-dialog/user-register-dialog.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, AfterViewInit {
  users: UserDto[];
  statusFilter: FormControl;
  userStatus = [
    {text: 'Active', value: UserStatus.active},
    {text: 'Suspended', value: UserStatus.suspended}
  ];
  loading: boolean;

  displayedColumns: string[] = ['userName', 'email', 'firstName', 'lastName', 'role', 'actions'];

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    this.statusFilter = this.fb.control(UserStatus.active);
    this.getUsers();
  }

  ngAfterViewInit() {
    this.route.queryParams.subscribe(data => {
      if (data.newUser) {
        setTimeout(() => {
          this.onRegisterUserClick();
        }, 0);
      }
    });
  }

  getUsers() {
    this.loading = true;
    this.accountService.getUsers(this.statusFilter.value, UserRole.All)
      .subscribe(data => {
        this.users = data;
        this.loading = false;
      });
  }

  onSetRoleClick(user: UserDto) {
    const dialogRef = this.dialog.open(UserSetRoleDialogComponent, {
      data: user
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getUsers();
      }
    });
  }

  onInfoClick(user: UserDto) {
    const dialogRef = this.dialog.open(UserInfoDialogComponent, {
      data: user
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getUsers();
      }
    });
  }

  onActivateClick(user: UserDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Activate User',
        confirmationText: `Are you sure you want to activate user ${user.userName}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountService.activate(user.id)
          .subscribe(() => {
            this.snackbar.open('User has been activated');

            this.getUsers();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onSuspendClick(user: UserDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Suspend User',
        confirmationText: `Are you sure you want to suspend user ${user.userName}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountService.suspend(user.id)
          .subscribe(() => {
            this.snackbar.open('User has been suspended');

            this.getUsers();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onDeleteClick(user: UserDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Delete User',
        confirmationText: `Are you sure you want to remove user ${user.userName}?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.accountService.remove(user.id)
          .subscribe(() => {
            this.snackbar.open('User has been removed');

            this.getUsers();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onRegisterUserClick() {
    const dialogRef = this.dialog.open(UserRegisterDialogComponent);

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getUsers();
      }
    });
  }

  onStatusFilterChanged() {
    this.getUsers();
  }

}
