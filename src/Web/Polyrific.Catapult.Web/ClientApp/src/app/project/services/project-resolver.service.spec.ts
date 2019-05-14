import { TestBed } from '@angular/core/testing';

import { ProjectResolverService } from './project-resolver.service';
import { ProjectService } from '@app/core';
import { ApiService } from '@app/core/services/api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('ProjectResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule,
      RouterTestingModule
    ],
    providers: [
      ProjectService,
      ApiService,
      ProjectResolverService
    ]
  }));

  it('should be created', () => {
    const service: ProjectResolverService = TestBed.get(ProjectResolverService);
    expect(service).toBeTruthy();
  });
});
