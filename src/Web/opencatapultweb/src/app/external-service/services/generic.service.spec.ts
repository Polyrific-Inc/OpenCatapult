import { TestBed } from '@angular/core/testing';

import { GenericService } from './generic.service';
import { FormsModule, FormBuilder } from '@angular/forms';

describe('GenericService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      FormsModule,
    ],
    providers: [
      GenericService,
      FormBuilder
    ]
  }));

  it('should be created', () => {
    const service: GenericService = TestBed.get(GenericService);
    expect(service).toBeTruthy();
  });
});
