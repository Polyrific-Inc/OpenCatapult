import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Config, ConfigService } from '../../config/config.service';

@Injectable()
export class ApiService {
  config: Config;
  constructor(
    private http: HttpClient,
    private configService: ConfigService
  ) {
    this.config = configService.getConfig();
    if (!this.config) {
      this.config = {
        apiUrl: 'http://localhost',
        environmentName: 'local-dev'
      };
    }
  }

  private formatErrors(error: HttpErrorResponse) {
    if (error.error) {
      if (typeof error.error === 'string') {
        try {
          const parsedError = JSON.parse(error.error);

          if (parsedError.Message) {
            return throwError(parsedError.Message);
          }
        } catch {
          // do nothing
        }

        return throwError(error.error);
      } else if (error.error[''] && Array.isArray(error.error[''])) {
        return throwError(error.error[''].join('\n'));
      } else if (Array.isArray(error.error)) {
        return throwError(error.error.join('\n'));
      } else if (error.error.message) {
        return throwError(error.error.message);
      }
    }

    return throwError(error.message);
  }

  get<T>(path: string, params: HttpParams = new HttpParams()): Observable<T> {
    return this.http.get<T>(`${this.config.apiUrl}/${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  getString(path: string, params: HttpParams = new HttpParams()): Observable<string> {
    return this.http.get(`${this.config.apiUrl}/${path}`, { responseType: 'text', ...params})
      .pipe(catchError(this.formatErrors));
  }

  put<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.put<T>(
      `${this.config.apiUrl}/${path}`,
      body
    ).pipe(catchError(this.formatErrors));
  }

  putString(path: string, body: Object = {}): Observable<string> {
    return this.http.put(
      `${this.config.apiUrl}/${path}`,
      body,
      { responseType: 'text' }
    ).pipe(catchError(this.formatErrors));
  }

  post<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.post<T>(
      `${this.config.apiUrl}/${path}`,
      body
    ).pipe(catchError(this.formatErrors));
  }

  postString(path: string, body: Object = {}): Observable<string> {
    return this.http.post(
      `${this.config.apiUrl}/${path}`,
      body,
      {
        responseType: 'text'
      }
    ).pipe(catchError(this.formatErrors));
  }

  delete<T>(path): Observable<T> {
    return this.http.delete<T>(
      `${this.config.apiUrl}/${path}`
    ).pipe(catchError(this.formatErrors));
  }
}
