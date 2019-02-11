import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from '../core/auth/auth.service';
import { Router } from '@angular/router';
import { AuthorizePolicy } from '../core/auth/authorize-policy';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  isLoggedIn$: Observable<boolean>;
  authorizePolicyEnum = AuthorizePolicy;

  constructor(
      private authService: AuthService,
      private router: Router
    ) { }

  ngOnInit() {
    this.isLoggedIn$ = this.authService.isLoggedIn;
  }

  onLogout(){
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  isMenuShown(authorizePolicy : AuthorizePolicy) {
    return this.authService.checkRoleAuthorization(authorizePolicy, null);
  }
}
