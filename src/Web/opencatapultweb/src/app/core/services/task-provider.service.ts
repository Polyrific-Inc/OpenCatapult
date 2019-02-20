import { Injectable } from '@angular/core';
import { TaskProviderDto } from '../models/task-provider/task-provider-dto';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

@Injectable()
export class TaskProviderService {

  constructor(private apiService: ApiService) { }

  getTaskProviderByName(name: string): Observable<TaskProviderDto> {
    return this.apiService.get<TaskProviderDto>(`provider/name/${name}`);
  }
}
