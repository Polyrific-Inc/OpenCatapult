import { TestBed } from '@angular/core/testing';

import { HelpContextService } from './help-context.service';

describe('HelpContextService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HelpContextService = TestBed.get(HelpContextService);
    expect(service).toBeTruthy();
  });
});
