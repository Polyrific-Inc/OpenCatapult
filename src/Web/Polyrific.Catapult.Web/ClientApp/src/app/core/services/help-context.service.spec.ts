import { TestBed } from '@angular/core/testing';

import { HelpContextService } from './help-context.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('HelpContextService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      ApiService,
      HelpContextService,
    ]
  }));

  it('should be created', () => {
    const service: HelpContextService = TestBed.get(HelpContextService);
    expect(service).toBeTruthy();
  });
});
