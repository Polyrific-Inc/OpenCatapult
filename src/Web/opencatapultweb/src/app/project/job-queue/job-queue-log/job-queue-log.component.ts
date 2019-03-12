import { Component, OnInit } from '@angular/core';
import { JobDto, JobQueueService, ProjectService } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { JobLogDto } from '@app/core/models/job-queue/job-log-dto';
import { JobStatus } from '@app/core/enums/job-status';

@Component({
  selector: 'app-job-queue-log',
  templateUrl: './job-queue-log.component.html',
  styleUrls: ['./job-queue-log.component.css']
})
export class JobQueueLogComponent implements OnInit {queueId: number;
  projectId: number;
  job: JobDto;
  log$: Observable<JobLogDto>;
  listened: boolean;
  logReceived: boolean;
  constructor(
    private route: ActivatedRoute,
    private jobQueueService: JobQueueService
  ) { }

  ngOnInit() {
    this.getQueue();
  }

  getQueue() {
    this.route.data.subscribe((data: {jobQueue: JobDto}) => {
      if (data.jobQueue.jobTasksStatus) {
        data.jobQueue.jobTasksStatus.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
      }

      this.job = data.jobQueue;
      this.queueId = this.job.id;
      this.projectId = this.job.projectId;

      if (!this.listened && (data.jobQueue.status === JobStatus.Queued || data.jobQueue.status === JobStatus.Processing)) {
        this.listenQueueLog();
      }
    });
  }

  listenQueueLog() {
    const self = this;
    this.log$ = this.jobQueueService.listenJobQueueLog(this.projectId, this.queueId);
    let previousTask = '';
    this.log$.subscribe((log) => {
      if (previousTask !== log.taskName) {
        previousTask = log.taskName;
        self.getQueue();
      }
    });
    this.listened = true;
  }

}
