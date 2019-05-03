import { TestBed } from '@angular/core/testing';

import { VersionService } from './version.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('VersionService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [VersionService, ApiService],
    imports: [HttpClientTestingModule]
  }));

  it('should be created', () => {
    const service: VersionService = TestBed.get(VersionService);
    expect(service).toBeTruthy();
  });
});
