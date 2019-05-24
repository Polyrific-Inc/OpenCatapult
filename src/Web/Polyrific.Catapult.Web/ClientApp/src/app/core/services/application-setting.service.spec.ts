import { TestBed } from '@angular/core/testing';

import { ApplicationSettingService } from './application-setting.service';
import { ApiService } from './api.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ApplicationSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ApiService,
      ApplicationSettingService
    ],
    imports: [
      HttpClientTestingModule
    ]
  }));

  it('should be created', () => {
    const service: ApplicationSettingService = TestBed.get(ApplicationSettingService);
    expect(service).toBeTruthy();
  });
});
