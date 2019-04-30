import { Injectable } from '@angular/core';
import * as jsYaml from 'js-yaml';

@Injectable()
export class YamlService {

  constructor() { }

  deserialize(filePath) {
    const deserializedObj = jsYaml.safeLoad(filePath);

    // rename snake names to camel case
    this.modifyObjectProps(deserializedObj);

    return deserializedObj;
  }

  private modifyObjectProps(obj) {
    if (obj) {
      for (const key of Object.keys(obj)) {
        const currentObj = obj[key];
        if (typeof currentObj === 'object') {
          this.modifyObjectProps(obj[key]);
        } else if (Array.isArray(currentObj)) {
          for (const item of currentObj) {
            this.modifyObjectProps(item);
          }
        }

        this.renameKey(obj, key);
      }
    }
  }

  private renameKey(obj, oldKey) {
    if (oldKey.indexOf('-') >= 0) {
      const newKey = this.snakeToCamel(oldKey);
      if (oldKey !== newKey) {
          Object.defineProperty(obj, newKey,
              Object.getOwnPropertyDescriptor(obj, oldKey));
          delete obj[oldKey];
      }
    }
  }

  private snakeToCamel(str) {
    return str.replace(/(\-\w)/g, function(m) {
      return m[1].toUpperCase();
    });
  }
}
