import { TestBed } from '@angular/core/testing';

import { SignalrService } from './signalr.service';
import { AuthService } from '../auth/auth.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SignalrService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      SignalrService,
      AuthService
    ]
  }));

  it('should be created', () => {
    const service: SignalrService = TestBed.get(SignalrService);
    expect(service).toBeTruthy();
  });
});
