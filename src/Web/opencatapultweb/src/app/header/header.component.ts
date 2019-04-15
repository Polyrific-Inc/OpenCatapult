import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthorizePolicy } from '../core/auth/authorize-policy';
import { MatSidenav } from '@angular/material';
import { AccountService, ManagedFileService } from '@app/core';

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

  constructor(
      private authService: AuthService,
      private accountService: AccountService,
      private router: Router,
      private managedFileService: ManagedFileService
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
          this.greetingsName = user.email;
        }

        this.accountService.getUserByEmail(user.email)
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
}
