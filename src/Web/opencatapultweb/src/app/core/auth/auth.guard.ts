import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';

import { AuthService } from './auth.service';
import { ProjectService } from '../services/project.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(
    private authService: AuthService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
      const currentUser = this.authService.currentUserValue;
        if (currentUser) {
          if (route.params.projectId) {
            this.projectService.currentProjectId = +route.params.projectId;
          }

          // check if route is restricted by role
          if (route.data.authPolicy &&
            !this.authService.checkRoleAuthorization(route.data.authPolicy, this.projectService.currentProjectId)) {
              // role not authorized so redirect to unauthorized error page
              this.router.navigate(['/unauthorized']);
              return false;
          }

          // authorized so return true
          return true;
        }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }

    canActivateChild (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
      return this.canActivate(route, state);
    }
}
