import { TestBed } from '@angular/core/testing';

import { SnackbarService } from './snackbar.service';
import { MatSnackBarModule } from '@angular/material';

describe('SnackbarService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      MatSnackBarModule
    ],
    providers: [ SnackbarService ]
  }));

  it('should be created', () => {
    const service: SnackbarService = TestBed.get(SnackbarService);
    expect(service).toBeTruthy();
  });
});
