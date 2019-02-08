import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { User } from './user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {  
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  get isLoggedIn() {
    return this.currentUser.pipe(map((currentUser: User) => {
      return currentUser != null;
    }));
  }

  constructor(
    private http: HttpClient
  ) {    
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue(): User {
      return this.currentUserSubject.value;
  }

  login(user: User){
    return this.http.post(`${environment.apiUrl}/Token`, { Email: user.email, Password: user.password },
    {      
      responseType: 'text'
    })
        .pipe(
          map(token => {
            // login successful if there's a jwt token in the response
            if (token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                user.token = token;
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
            }

            return user;
        }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
