import { Component, OnInit, Input, ViewChild, OnDestroy } from '@angular/core';
import { JobQueueDto, JobQueueService, JobStatus } from '@app/core';
import { MatPaginator, MatTableDataSource, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-job-queue-list',
  templateUrl: './job-queue-list.component.html',
  styleUrls: ['./job-queue-list.component.css']
})
export class JobQueueListComponent implements OnInit, OnDestroy {
  usePaging: boolean;
  paginator: MatPaginator;
  sort: MatSort;
  dataSource: MatTableDataSource<JobQueueDto>;
  routerSubscription: Subscription;

  displayedColumns: string[] = ['jobDefinitionName', 'status', 'created', 'updated', 'actions'];

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.dataSource.sort = this.sort;
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.dataSource.paginator = this.paginator;
  }

  constructor(
    private jobQueueService: JobQueueService,
    private route: ActivatedRoute) {
   }

  ngOnInit() {
    this.routerSubscription = this.route.url.subscribe(currentUrl => {
      const currentRoute = currentUrl.length > 0 ? currentUrl[currentUrl.length - 1].path : '';
      let filteredJobs;

      switch (currentRoute.toLowerCase()) {
        case 'pending':
          filteredJobs = this.jobQueueService.jobs.filter(job => job.status === JobStatus.Pending);
          break;
        case 'past':
          filteredJobs = this.jobQueueService.jobs;
          this.usePaging = true;
          break;
        default:
          filteredJobs = this.jobQueueService.jobs.filter(job => job.status === JobStatus.Queued || job.status === JobStatus.Processing);
        break;
      }

      this.dataSource = new MatTableDataSource<JobQueueDto>(filteredJobs);
    });
  }

  ngOnDestroy() {
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }
}
