import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ProjectTemplateService {

  constructor(
    private http: HttpClient
    ) { }

  getTemplate(template: string): Observable<string> {
    return this.http.get(`assets/template/${template}`, {
      responseType: 'text'
    }).pipe(catchError((err: HttpErrorResponse) => {
      console.log(err);
      if (err.status === 404) {
        return throwError(`Template ${template} is not found in the server`);
      }

      return throwError(err.message);
    }));
  }
}
