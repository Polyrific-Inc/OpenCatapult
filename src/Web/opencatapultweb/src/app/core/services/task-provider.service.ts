import { Injectable } from '@angular/core';
import { TaskProviderDto } from '../models/task-provider/task-provider-dto';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { JobTaskDefinitionType } from '../enums/job-task-definition-type';
import { ProviderType } from '../enums/provider-type';

@Injectable()
export class TaskProviderService {

  constructor(private apiService: ApiService) { }

  getTaskProviderByName(name: string): Observable<TaskProviderDto> {
    return this.apiService.get<TaskProviderDto>(`provider/name/${name}`);
  }

  getTaskProviders() {
    return this.apiService.get<TaskProviderDto[]>('provider');
  }

  getTaskProviderType(taskType: string) {
    switch (taskType) {
      case JobTaskDefinitionType.Clone:
      case JobTaskDefinitionType.Push:
      case JobTaskDefinitionType.Merge:
        return ProviderType.RepositoryProvider;
      case JobTaskDefinitionType.Generate:
        return ProviderType.GeneratorProvider;
      case JobTaskDefinitionType.Build:
        return ProviderType.BuildProvider;
      case JobTaskDefinitionType.DeployDb:
        return ProviderType.DatabaseProvider;
      case JobTaskDefinitionType.Deploy:
        return ProviderType.HostingProvider;
      case JobTaskDefinitionType.PublishArtifact:
        return ProviderType.StorageProvider;
      case JobTaskDefinitionType.Test:
        return ProviderType.TestProvider;
      default:
        return '';
    }
  }
}
