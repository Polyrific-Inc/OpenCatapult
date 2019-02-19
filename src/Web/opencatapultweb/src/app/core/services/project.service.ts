import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { ProjectDto } from '../models/project/project-dto';
import { NewProjectDto } from '../models/project/new-project-dto';

@Injectable()
export class ProjectService {
  constructor(private apiService: ApiService) { }

  getProjects(status: string, getAll: boolean) : Observable<ProjectDto[]> {
    return this.apiService.get<ProjectDto[]>(`project?status=${status}&getAll=${getAll}`);
  }

  getProject(projectId: number) : Observable<ProjectDto> {
    return this.apiService.get<ProjectDto>(`project/${projectId}`);
  }

  updateProject(project: ProjectDto) {
    return this.apiService.put(`project/${project.id}`, project);
  }

  createProject(project: NewProjectDto) {
    return this.apiService.post('project', project);
  }

  deleteProject(projectId: number) {
    return this.apiService.delete(`project/${projectId}`);
  }
}
