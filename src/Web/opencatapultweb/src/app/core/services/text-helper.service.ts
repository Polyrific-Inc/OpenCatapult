import { Injectable } from '@angular/core';

@Injectable()
export class TextHelperService {

  constructor() { }

  humanizeText(text: string): string {
    return text
        .replace(/([A-Z])/g, ' $1')
        .replace(/^[\s_]+|[\s_]+$/g, '')
        .replace(/[_\s]+/g, ' ')
        .replace(/^[\s-]+|[\s-]+$/g, '')
        .replace(/[-\s]+/g, ' ')
        .replace(/^[a-z]/, function(m) { return m.toUpperCase(); });
  }

  validateEmail(email) {
    // tslint:disable-next-line:max-line-length
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
  }
}
