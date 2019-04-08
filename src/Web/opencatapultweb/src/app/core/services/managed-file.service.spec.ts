import { TestBed } from '@angular/core/testing';

import { ManagedFileService } from './managed-file.service';
import { ApiService } from './api.service';
import { ConfigService } from '@app/config/config.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ManagedFileService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      ManagedFileService,
      ApiService,
      ConfigService
    ]
  }));

  it('should be created', () => {
    const service: ManagedFileService = TestBed.get(ManagedFileService);
    expect(service).toBeTruthy();
  });
});
