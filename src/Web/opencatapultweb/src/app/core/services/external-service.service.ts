import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { ExternalServiceDto } from '../models/external-service/external-service-dto';

@Injectable()
export class ExternalServiceService {

  constructor(private apiService: ApiService) { }

  getExternalServiceByName(name: string): Observable<ExternalServiceDto> {
    return this.apiService.get<ExternalServiceDto>(`service/name/${name}`);
  }

  getExternalServices() {
    return this.apiService.get<ExternalServiceDto[]>(`service`);
  }
}
