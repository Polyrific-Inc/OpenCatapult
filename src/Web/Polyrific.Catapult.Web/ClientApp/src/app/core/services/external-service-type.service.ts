import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { ExternalServiceTypeDto } from '../models/external-service/external-service-type-dto';

@Injectable()
export class ExternalServiceTypeService {

  constructor(private api: ApiService) { }

  getExternalServiceType(id: number) {
    return this.api.get<ExternalServiceTypeDto>(`serviceType/${id}`);
  }

  getExternalServiceTypeByName(name: string) {
    return this.api.get<ExternalServiceTypeDto>(`serviceType/name/${name}`);
  }

  getExternalServiceTypes(includeProperties: boolean) {
    let path = 'serviceType';

    if (includeProperties) {
      path += '?includeProperties=true';
    }

    return this.api.get<ExternalServiceTypeDto[]>(path);
  }
}
