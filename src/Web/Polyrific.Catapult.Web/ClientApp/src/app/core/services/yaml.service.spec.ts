import { TestBed } from '@angular/core/testing';

import { YamlService } from './yaml.service';

describe('YamlService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      YamlService
    ]
  }));

  it('should be created', () => {
    const service: YamlService = TestBed.get(YamlService);
    expect(service).toBeTruthy();
  });
});
