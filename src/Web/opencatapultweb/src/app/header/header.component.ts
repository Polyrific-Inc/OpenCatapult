import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthorizePolicy } from '../core/auth/authorize-policy';
import { MatSidenav } from '@angular/material';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Input() sidenav: MatSidenav;
  authorizePolicyEnum = AuthorizePolicy;
  greetingsName: string;

  constructor(
      private authService: AuthService,
      private router: Router
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
      }
    });
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isMenuShown(authorizePolicy: AuthorizePolicy) {
    return this.authService.checkRoleAuthorization(authorizePolicy, null);
  }
}
