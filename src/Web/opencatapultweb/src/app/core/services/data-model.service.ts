import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DataModelDto } from '../models/data-model/data-model-dto';
import { CreateDataModelDto } from '../models/data-model/create-data-model-dto';
import { CreateDataModelPropertyDto } from '../models/data-model/create-data-model-property-dto';
import { DataModelPropertyDto } from '../models/data-model/data-model-property-dto';

@Injectable()
export class DataModelService {

  constructor(private apiService: ApiService) { }

  getDataModels(projectId: number, includeProperties: boolean): Observable<DataModelDto[]> {
    const params = new HttpParams().set('includeProperties', includeProperties.toString());
    return this.apiService.get<DataModelDto[]>(`project/${projectId}/model`, params);
  }

  createDataModel(projectId: number, model: CreateDataModelDto) {
    return this.apiService.post<DataModelDto>(`project/${projectId}/model`, model);
  }

  updateDataModel(projectId: number, modelId: number, model: DataModelDto) {
    return this.apiService.put(`project/${projectId}/model/${modelId}`, model);
  }

  deleteDataModel(projectId: number, modelId: number) {
    return this.apiService.delete(`project/${projectId}/model/${modelId}`);
  }

  deleteDataModels(projectId: number, modelIds: number[]) {
    let queryParams = '';
    for (let i = 0; i < modelIds.length; i++) {
      queryParams += `modelIds=${modelIds[i]}`;

      if (i + 1 < modelIds.length) {
        queryParams += '&';
      }
    }

    return this.apiService.delete(`project/${projectId}/model/bulkdelete?${queryParams}`);
  }

  createDataModelProperty(projectId: number, modelId: number, property: CreateDataModelPropertyDto) {
    return this.apiService.post<DataModelPropertyDto>(`project/${projectId}/model/${modelId}/property`, property);
  }

  getDataModelProperties(projectId: number, modelId: number) {
    return this.apiService.get<DataModelPropertyDto[]>(`project/${projectId}/model/${modelId}/property`);
  }

  updateDataModelProperty(projectId: number, modelId: number, property: DataModelPropertyDto) {
    return this.apiService.put(`project/${projectId}/model/${modelId}/property/${property.id}`, property);
  }

  deleteDataModelProperty(projectId: number, modelId: number, propertyId: number) {
    return this.apiService.delete(`project/${projectId}/model/${modelId}/property/${propertyId}`);
  }
}
