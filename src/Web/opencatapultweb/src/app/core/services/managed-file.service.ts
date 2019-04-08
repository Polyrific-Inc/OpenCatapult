import { Injectable } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ManagedFileDto } from '../models/managed-file/managed-file-dto';

@Injectable()
export class ManagedFileService {

  constructor(private sanitizer: DomSanitizer) { }

  getImagePath(managedFile: ManagedFileDto) {
    if (managedFile && managedFile.file) {
      return this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpeg;base64,' + managedFile.file);
    }

    return null;
  }
}
