import { Injectable } from '@angular/core';
import { ProjectDto, ProjectService } from '@app/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of, EMPTY } from 'rxjs';
import { mergeMap } from 'rxjs/operators';

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
          return of(project);
        } else {
          this.router.navigateByUrl(`/project/${projectId}/error`);
          return EMPTY;
        }
      }));
  }
}
