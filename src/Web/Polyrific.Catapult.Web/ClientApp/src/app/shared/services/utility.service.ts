import { Injectable } from '@angular/core';

@Injectable()
export class UtilityService {

  constructor() { }

  convertStringToBoolean(dictionary: { [key: string]: string}): { [key: string]: any } {
    if (dictionary) {
      const convertedDictionary: { [key: string]: any } = {};

      for (const key in dictionary) {
        if (typeof key === 'string') {
          const value = dictionary[key];
          if (typeof value === 'string' && (value.toLowerCase() === 'true' || value.toLowerCase() === 'false')) {
            convertedDictionary[key] = value === 'true';
          } else {
            convertedDictionary[key] = value;
          }
        }
      }

      return convertedDictionary;
    }

    return dictionary;
  }
}
