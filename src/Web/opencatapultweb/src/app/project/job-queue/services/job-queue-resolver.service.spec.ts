import { TestBed } from '@angular/core/testing';

import { JobQueueResolverService } from './job-queue-resolver.service';
import { CoreModule } from '@app/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('JobQueueResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      CoreModule,
      HttpClientTestingModule,
      RouterTestingModule
    ],
    providers: [
      JobQueueResolverService
    ]
  }));

  it('should be created', () => {
    const service: JobQueueResolverService = TestBed.get(JobQueueResolverService);
    expect(service).toBeTruthy();
  });
});
