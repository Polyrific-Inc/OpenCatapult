import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, } from 'rxjs/operators';
import { User } from './user';
import * as jwt_decode from 'jwt-decode';
import { AuthorizePolicy } from './authorize-policy';
import { Role } from './role';
import { ProjectMemberRole } from './project-member-role';
import { Config, ConfigService } from '../../config/config.service';

@Injectable()
export class AuthService {
  private config: Config;
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  get isLoggedIn() {
    return this.currentUser.pipe(map((currentUser: User) => {
      return currentUser != null;
    }));
  }

  constructor(
    private configService: ConfigService,
    private http: HttpClient) {
      this.config = this.configService.getConfig();
      this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
      this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }

  login(user: User) {
    if (!this.config) {
      this.config = this.configService.getConfig();
    }

    return this.http.post(`${this.config.apiUrl}/Token`, { Email: user.email, Password: user.password },
    {
      responseType: 'text'
    }).pipe(map(this.storeToken(user)));
  }

  refreshSession() {
    return this.http.get(`${this.config.apiUrl}/Token/refresh`,
    {
      responseType: 'text'
    }).pipe(map(this.storeToken(this.currentUserValue)));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  checkRoleAuthorization(authPolicy: AuthorizePolicy, projectId: number): boolean {
    const currentUser = this.currentUserValue;
    if (currentUser.role === Role.Administrator) {
      return true;
    }

    switch (authPolicy) {
      case AuthorizePolicy.UserRoleAdminAccess:
        return currentUser.role === Role.Administrator;
      case AuthorizePolicy.UserRoleBasicAccess:
        return [Role.Administrator, Role.Basic].includes(Role[currentUser.role]);
      case AuthorizePolicy.UserRoleGuestAccess:
        return [Role.Administrator, Role.Basic, Role.Guest].includes(Role[currentUser.role]);
      case AuthorizePolicy.ProjectAccess:
      case AuthorizePolicy.ProjectMemberAccess:
        return projectId > 0 && currentUser.projects && currentUser.projects.some(p => p.projectId === projectId);
      case AuthorizePolicy.ProjectContributorAccess:
        return projectId > 0 && currentUser.projects && currentUser.projects.some(p => p.projectId === projectId &&
          [ProjectMemberRole.Owner, ProjectMemberRole.Maintainer, ProjectMemberRole.Contributor].includes(ProjectMemberRole[p.memberRole]));
      case AuthorizePolicy.ProjectMaintainerAccess:
        return projectId > 0 && currentUser.projects && currentUser.projects.some(p => p.projectId === projectId &&
          [ProjectMemberRole.Owner, ProjectMemberRole.Maintainer].includes(ProjectMemberRole[p.memberRole]));
      case AuthorizePolicy.ProjectOwnerAccess:
        return projectId > 0 && currentUser.projects && currentUser.projects.some(p => p.projectId === projectId &&
          p.memberRole === ProjectMemberRole.Owner);
      default:
      return false;
     }
  }

  private storeToken(user: User) {
    return (token: string) => {
            // login successful if there's a jwt token in the response
        if (token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          user.token = token;
          const decodedToken = this.getDecodedAccessToken(token);

          if (decodedToken.Projects) {
            user.projects = JSON.parse(decodedToken.Projects).map(pm => ({
                projectId: pm.ProjectId,
                projectName: pm.ProjectName,
                memberRole: pm.MemberRole
              }));
          }

          if (decodedToken.hasOwnProperty('http://schemas.microsoft.com/ws/2008/06/identity/claims/role')) {
            user.role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname')) {
            user.firstName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'];
          }

          if (decodedToken.hasOwnProperty('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname')) {
            user.lastName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'];
          }

          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
      }

      return user;
    };
  }

  private getDecodedAccessToken(token: string): any {
    try {
        return jwt_decode(token);
    } catch (Error) {
        return null;
    }
  }
}
