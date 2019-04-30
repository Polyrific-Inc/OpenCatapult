import { TestBed } from '@angular/core/testing';

import { JobDefinitionService } from './job-definition.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('JobDefinitionService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      JobDefinitionService,
      ApiService
    ]
  }));

  it('should be created', () => {
    const service: JobDefinitionService = TestBed.get(JobDefinitionService);
    expect(service).toBeTruthy();
  });
});
