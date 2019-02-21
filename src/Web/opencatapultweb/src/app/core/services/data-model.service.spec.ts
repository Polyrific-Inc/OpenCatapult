import { TestBed } from '@angular/core/testing';

import { DataModelService } from './data-model.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiService } from './api.service';

describe('DataModelService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      DataModelService,
      ApiService
    ]
  }));

  it('should be created', () => {
    const service: DataModelService = TestBed.get(DataModelService);
    expect(service).toBeTruthy();
  });
});
