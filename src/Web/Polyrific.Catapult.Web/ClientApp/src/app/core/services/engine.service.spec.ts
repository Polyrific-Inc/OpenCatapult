import { TestBed } from '@angular/core/testing';

import { EngineService } from './engine.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('EngineService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      EngineService,
      ApiService
    ]
  }));

  it('should be created', () => {
    const service: EngineService = TestBed.get(EngineService);
    expect(service).toBeTruthy();
  });
});
