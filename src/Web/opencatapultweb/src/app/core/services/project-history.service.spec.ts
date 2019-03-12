import { TestBed } from '@angular/core/testing';

import { ProjectHistoryService } from './project-history.service';
import { AuthService } from '../auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ProjectHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ProjectHistoryService,
      AuthService
    ],
    imports: [
      HttpClientTestingModule
    ]
  }));

  it('should be created', () => {
    const service: ProjectHistoryService = TestBed.get(ProjectHistoryService);
    expect(service).toBeTruthy();
  });
});
