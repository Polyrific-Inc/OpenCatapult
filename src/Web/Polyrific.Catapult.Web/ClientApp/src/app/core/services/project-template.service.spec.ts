import { TestBed } from '@angular/core/testing';

import { ProjectTemplateService } from './project-template.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ProjectTemplateService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [ ProjectTemplateService, ApiService ]
  }));

  it('should be created', () => {
    const service: ProjectTemplateService = TestBed.get(ProjectTemplateService);
    expect(service).toBeTruthy();
  });
});
