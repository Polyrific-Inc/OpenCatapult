import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable, Subject, BehaviorSubject, of } from 'rxjs';
import { ExternalServiceDto } from '../models/external-service/external-service-dto';
import { UpdateExternalServiceDto } from '../models/external-service/update-external-service-dto';
import { CreateExternalServiceDto } from '../models/external-service/create-external-service-dto';
import { concatMap } from 'rxjs/operators';

@Injectable()
export class ExternalServiceService {
  private currentExternalServices = new BehaviorSubject<ExternalServiceDto[]>([]);
  public externalServices: Observable<ExternalServiceDto[]> = this.currentExternalServices.asObservable();

  constructor(private api: ApiService) { }

  getExternalServiceByName(name: string): Observable<ExternalServiceDto> {
    return this.api.get<ExternalServiceDto>(`service/name/${name}`);
  }

  getExternalService(id: number) {
    return this.api.get<ExternalServiceDto>(`service/${id}`);
  }

  getExternalServices() {
    return this.api.get<ExternalServiceDto[]>(`service`)
      .pipe(concatMap((data) => {
        this.currentExternalServices.next(data);
        return of(data);
      }));
  }

  createExternalService(dto: CreateExternalServiceDto) {
    return this.api.post(`service`, dto);
  }

  updateExternalService(externalServiceId: number, dto: UpdateExternalServiceDto) {
    return this.api.put(`service/${externalServiceId}`, dto);
  }

  deleteExternalService(externalServiceId: number) {
    return this.api.delete(`service/${externalServiceId}`);
  }
}
