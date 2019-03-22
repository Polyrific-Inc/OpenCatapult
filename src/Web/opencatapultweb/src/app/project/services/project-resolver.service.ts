import { Injectable } from '@angular/core';
import { ProjectDto, ProjectService } from '@app/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { ProjectDetailComponent } from '../project-detail/project-detail.component';

@Injectable()
export class ProjectResolverService implements Resolve<ProjectDto> {

  constructor(
    private projectService: ProjectService,
    private router: Router
    ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): ProjectDto | Observable<ProjectDto> | Promise<ProjectDto> {
    const projectId = +route.params.projectId;

    return this.projectService.getProject(projectId)
      .pipe(mergeMap(project => {
        if (project) {
          if (project.status === 'archived' && route.component === ProjectDetailComponent) {
            this.router.navigateByUrl(`/project/archive/${projectId}`);
            return EMPTY;
          }

          if (project.status === 'deleting' && route.component === ProjectDetailComponent) {
            this.router.navigateByUrl(`/project/deleting/${projectId}`);
            return EMPTY;
          }

          return of(project);
        } else {
          this.router.navigateByUrl(`/project/${projectId}/error`);
          return EMPTY;
        }
      }));
  }
}
