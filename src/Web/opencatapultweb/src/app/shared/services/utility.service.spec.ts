import { TestBed } from '@angular/core/testing';

import { UtilityService } from './utility.service';

describe('UtilityService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [UtilityService]
  }));

  it('should be created', () => {
    const service: UtilityService = TestBed.get(UtilityService);
    expect(service).toBeTruthy();
  });
});
