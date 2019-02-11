import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient
  ) {}

  private formatErrors(error: HttpErrorResponse) {    
    if (error.error && typeof error.error === "string")
      return throwError(error.error);
    else
      return throwError(error.message);
  }

  get<T>(path: string, params: HttpParams = new HttpParams()): Observable<T> {
    return this.http.get<T>(`${environment.apiUrl}/${path}`, { params })
      .pipe(catchError(this.formatErrors));
  }

  put<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.put<T>(
      `${environment.apiUrl}/${path}`,
      JSON.stringify(body)
    ).pipe(catchError(this.formatErrors));
  }

  post<T>(path: string, body: Object = {}): Observable<T> {
    return this.http.post<T>(
      `${environment.apiUrl}/${path}`,
      JSON.stringify(body)
    ).pipe(catchError(this.formatErrors));
  }

  delete<T>(path): Observable<T> {
    return this.http.delete<T>(
      `${environment.apiUrl}/${path}`
    ).pipe(catchError(this.formatErrors));
  }
}