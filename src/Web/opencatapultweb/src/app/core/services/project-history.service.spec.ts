import { TestBed } from '@angular/core/testing';

import { ProjectHistoryService } from './project-history.service';

describe('ProjectHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ProjectHistoryService
    ]
  }));

  it('should be created', () => {
    const service: ProjectHistoryService = TestBed.get(ProjectHistoryService);
    expect(service).toBeTruthy();
  });
});
