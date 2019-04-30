import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AuthService } from '../auth/auth.service';
import { Config, ConfigService } from '../../config/config.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    private config: Config;

    constructor(
      private configService: ConfigService,
      private authenticationService: AuthService) {
        this.config = this.configService.getConfig();
      }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if (!this.config) {
          this.config = this.configService.getConfig();
        }

        // add authorization header with jwt token if available
        const currentUser = this.authenticationService.currentUserValue;
        if (this.config && request.url.startsWith(this.config.apiUrl) && currentUser && currentUser.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.token}`
                }
            });
        }

        return next.handle(request);
    }
}
