import { Injectable } from '@angular/core';
import { JobQueueService, JobQueueDto } from '@app/core';
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { mergeMap } from 'rxjs/operators';
import { of, EMPTY, Observable } from 'rxjs';

@Injectable()
export class JobQueueResolverService implements Resolve<JobQueueDto> {

  constructor(
    private jobQueueService: JobQueueService,
    private router: Router
    ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): JobQueueDto | Observable<JobQueueDto> | Promise<JobQueueDto> {
    const projectId = +route.parent.parent.params.projectId;
    const jobQueueId = +route.params.id;

    return this.jobQueueService.getJobQueue(projectId, jobQueueId)
      .pipe(mergeMap(job => {
        if (job) {
          return of(job);
        } else {
          this.router.navigateByUrl(`/project/${projectId}/job-queue/${jobQueueId}/error`);
          return EMPTY;
        }
      }));
  }
}
