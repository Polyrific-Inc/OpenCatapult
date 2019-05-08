import { TestBed } from '@angular/core/testing';

import { TaskProviderService } from './task-provider.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('TaskProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [TaskProviderService, ApiService]
  }));

  it('should be created', () => {
    const service: TaskProviderService = TestBed.get(TaskProviderService);
    expect(service).toBeTruthy();
  });
});
