import { TestBed } from '@angular/core/testing';

import { ExternalServiceTypeService } from './external-service-type.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('ExternalServiceTypeService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      ExternalServiceTypeService,
      ApiService
    ]}));

  it('should be created', () => {
    const service: ExternalServiceTypeService = TestBed.get(ExternalServiceTypeService);
    expect(service).toBeTruthy();
  });
});
