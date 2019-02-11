import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { ProjectDto } from '../models/ProjectDto';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private apiService: ApiService) { }

  getProjects(status: string, getAll: boolean) : Observable<ProjectDto[]> {
    return this.apiService.get<ProjectDto[]>(`project?status=${status}&getAll=${getAll}`);
  }
}
