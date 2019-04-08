import { TestBed } from '@angular/core/testing';

import { ManagedFileService } from './managed-file.service';
import { DomSanitizer } from '@angular/platform-browser';

describe('ManagedFileService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ManagedFileService,
      DomSanitizer
    ]
  }));

  it('should be created', () => {
    const service: ManagedFileService = TestBed.get(ManagedFileService);
    expect(service).toBeTruthy();
  });
});
