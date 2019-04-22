import { TestBed } from '@angular/core/testing';

import { TextHelperService } from './text-helper.service';

describe('TextHelperService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      TextHelperService
    ]
  }));

  it('should be created', () => {
    const service: TextHelperService = TestBed.get(TextHelperService);
    expect(service).toBeTruthy();
  });
});
