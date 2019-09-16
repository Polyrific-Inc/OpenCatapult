import { Component, OnInit, OnDestroy } from '@angular/core';
import { JobQueueService, JobQueueDto, JobQueueFilterType, ProjectService } from '@app/core';
import { JobStatus } from '@app/core/enums/job-status';
import { Router, ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-job-queue',
  templateUrl: './job-queue.component.html',
  styleUrls: ['./job-queue.component.css']
})
export class JobQueueComponent implements OnInit, OnDestroy {
  projectId: number;
  currentJobs: JobQueueDto[] = [];
  pendingJobs: JobQueueDto[] = [];
  pastJobs: JobQueueDto[];
  loading: boolean;
  private routerSubscription: Subscription;

  constructor(
    private jobQueueService: JobQueueService,
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.projectId = this.projectService.currentProjectId;

    this.routerSubscription = this.route.paramMap.subscribe(() => {
      if (this.router.url.toLowerCase().endsWith('past')) {
        this.getPastJobQueues();
      } else {
        this.getCurrentJobQueues();
      }
    });
  }

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }

  getCurrentJobQueues(): void {
    this.loading = true;
    this.jobQueueService.getJobQueues(this.projectId, JobQueueFilterType.current)
      .subscribe(jobs => {
        this.currentJobs = jobs.filter(job => job.status === JobStatus.Queued || job.status === JobStatus.Processing);
        this.pendingJobs = jobs.filter(job => job.status === JobStatus.Pending);
        this.loading = false;
      });
  }

  getPastJobQueues(): void {
    this.loading = true;
    this.jobQueueService.getJobQueues(this.projectId, JobQueueFilterType.past)
      .subscribe(jobs => {
        this.pastJobs = jobs;
        this.loading = false;
      });
  }

}
