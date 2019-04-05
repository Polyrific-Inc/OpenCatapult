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
}
