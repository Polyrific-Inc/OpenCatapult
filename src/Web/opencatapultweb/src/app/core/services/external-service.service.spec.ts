import { TestBed } from '@angular/core/testing';

import { ExternalServiceService } from './external-service.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ExternalServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      ExternalServiceService,
      ApiService
    ]
  }));

  it('should be created', () => {
    const service: ExternalServiceService = TestBed.get(ExternalServiceService);
    expect(service).toBeTruthy();
  });
});
