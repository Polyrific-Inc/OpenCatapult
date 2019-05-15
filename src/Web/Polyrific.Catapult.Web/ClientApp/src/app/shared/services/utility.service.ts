import { Injectable } from '@angular/core';
import { FormGroup, FormArray, AbstractControl } from '@angular/forms';

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

  markControlsAsTouched(group: FormGroup | FormArray, touched?: boolean): void {
    if (touched == null) {
      touched = true;
    }

    Object.keys(group.controls).forEach(field => {
        const control = group.get(field);

        if (control instanceof FormGroup || control instanceof FormArray) {
            this.markControlsAsTouched(control, touched);
        } else {
          if (touched) {
            control.markAsTouched({ onlySelf: true });
          } else {

            control.markAsUntouched({ onlySelf: true });
          }
        }
    });
  }
}
