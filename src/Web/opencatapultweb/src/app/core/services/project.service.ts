import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { ProjectDto } from '../models/project-dto';
import { tap, filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private currentProject = new BehaviorSubject<ProjectDto>(null);

  constructor(private apiService: ApiService) { }

  getProjects(status: string, getAll: boolean) : Observable<ProjectDto[]> {
    return this.apiService.get<ProjectDto[]>(`project?status=${status}&getAll=${getAll}`);
  }

  getProject(projectId: number) : Observable<ProjectDto> {
    let self = this;
    return this.apiService.get<ProjectDto>(`project/${projectId}`)
      .pipe(tap(data => 
        {
          self.currentProject.next(data);
        }));
  }

  getCurrentProject() : Observable<ProjectDto>
  {
    return this.currentProject.asObservable().pipe(filter(p => p != null));
  }
}
