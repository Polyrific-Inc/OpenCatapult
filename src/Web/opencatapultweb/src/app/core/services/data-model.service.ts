import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataModelDto } from '../models/data-model/data-model-dto';

@Injectable()
export class DataModelService {

  constructor(private apiService: ApiService) { }

  getDataModels(projectId: number, includeProperties: boolean): Observable<DataModelDto[]> {
    const params = new HttpParams().set('includeProperties', includeProperties.toString());
    return this.apiService.get<DataModelDto[]>(`project/${projectId}/model`, params);
  }
}
