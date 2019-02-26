import { TestBed } from '@angular/core/testing';

import { JobQueueService } from './job-queue.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('JobQueueService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      JobQueueService,
      ApiService
    ]
  }));

  it('should be created', () => {
    const service: JobQueueService = TestBed.get(JobQueueService);
    expect(service).toBeTruthy();
  });
});
