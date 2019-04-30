import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthorizePolicy } from '../core/auth/authorize-policy';
import { MatSidenav, MatDialog } from '@angular/material';
import { AccountService, ManagedFileService } from '@app/core';
import { HelpContextService } from '@app/core/services/help-context.service';
import { HelpContextDialogComponent } from '@app/help-context-dialog/help-context-dialog.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Input() sidenav: MatSidenav;
  authorizePolicyEnum = AuthorizePolicy;
  greetingsName: string;
  avatarImage: any;
  loading: boolean;

  constructor(
      private authService: AuthService,
      private accountService: AccountService,
      private router: Router,
      private dialog: MatDialog,
      private managedFileService: ManagedFileService,
      private helpContextService: HelpContextService
    ) { }

  ngOnInit() {
    this.authService.currentUser.subscribe(user => {
      if (user) {
        this.greetingsName = '';
        if (user.firstName) {
          this.greetingsName += user.firstName;
        }

        if (user.lastName) {
          this.greetingsName += ` ${user.lastName}`;
        }

        if (!this.greetingsName) {
          this.greetingsName = user.userName;
        }

        this.accountService.getUserByUserName(user.userName)
        .subscribe(data => {
          if (data.avatarFileId) {
            this.avatarImage = this.managedFileService.getImageUrl(data.avatarFileId);
          }
        });
      }
    });
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isQuickAddMenuShown() {
    return this.authService.checkRoleAuthorization(AuthorizePolicy.UserRoleAdminAccess, null) ||
      this.authService.checkRoleAuthorization(AuthorizePolicy.UserRoleBasicAccess, null);
  }

  onHelpClick() {
    this.loading = true;
    const section = this.helpContextService.getSectionByActiveRoute(this.router.url);
    this.helpContextService.getHelpContextsBySection(section)
      .subscribe(data => {
        this.loading = false;
        this.dialog.open(HelpContextDialogComponent, {
          data: data
        });
      });
  }
}
