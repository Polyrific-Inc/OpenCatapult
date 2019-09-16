import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertBoxService {
  private subject = new Subject<any>();

  constructor() { }

  setMessage(message: string, type: string) {
      this.subject.next({ type: type, text: message });
  }

  getMessage(): Observable<any> {
    return this.subject.asObservable();
  }
}
