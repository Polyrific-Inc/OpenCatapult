import { Injectable } from '@angular/core';
import { TaskProviderDto } from '../models/task-provider/task-provider-dto';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { JobTaskDefinitionType } from '../enums/job-task-definition-type';
import { ProviderType } from '../enums/provider-type';
import { RegisterTaskProviderDto } from '../models/task-provider/register-task-provider-dto';

@Injectable()
export class TaskProviderService {

  constructor(private api: ApiService) { }

  getTaskProviderByName(name: string): Observable<TaskProviderDto> {
    return this.api.get<TaskProviderDto>(`task-provider/name/${name}`);
  }

  getTaskProvider(id: number): Observable<TaskProviderDto> {
    return this.api.get<TaskProviderDto>(`task-provider/${id}`);
  }

  getTaskProviders(type: string) {
    return this.api.get<TaskProviderDto[]>(`task-provider/type/${type}`);
  }

  deleteTaskProvider(id: number) {
    return this.api.delete(`task-provider/${id}`);
  }

  registerTaskProvider(dto: RegisterTaskProviderDto) {
    return this.api.post('task-provider', dto);
  }

  getTaskProviderType(taskType: string) {
    switch (taskType) {
      case JobTaskDefinitionType.Pull:
      case JobTaskDefinitionType.Push:
      case JobTaskDefinitionType.Merge:
      case JobTaskDefinitionType.DeleteRepository:
        return ProviderType.RepositoryProvider;
      case JobTaskDefinitionType.Generate:
        return ProviderType.GeneratorProvider;
      case JobTaskDefinitionType.Build:
        return ProviderType.BuildProvider;
      case JobTaskDefinitionType.DeployDb:
        return ProviderType.DatabaseProvider;
      case JobTaskDefinitionType.Deploy:
      case JobTaskDefinitionType.DeleteHosting:
        return ProviderType.HostingProvider;
      case JobTaskDefinitionType.PublishArtifact:
        return ProviderType.StorageProvider;
      case JobTaskDefinitionType.Test:
        return ProviderType.TestProvider;
      case JobTaskDefinitionType.CustomTask:
        return ProviderType.GenericTaskProvider;
      default:
        return '';
    }
  }
}
