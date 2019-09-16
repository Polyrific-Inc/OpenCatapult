import { TestBed } from '@angular/core/testing';

import { AlertBoxService } from './alert-box.service';

describe('AlertBoxService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AlertBoxService = TestBed.get(AlertBoxService);
    expect(service).toBeTruthy();
  });
});
