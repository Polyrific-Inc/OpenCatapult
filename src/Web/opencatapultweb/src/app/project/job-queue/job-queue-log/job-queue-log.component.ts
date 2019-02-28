import { Component, OnInit } from '@angular/core';
import { JobDto, JobQueueService } from '@app/core';
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
    private jobQueueService: JobQueueService,
  ) { }

  ngOnInit() {
    this.queueId = this.route.snapshot.params.id;
    this.projectId = +this.route.parent.parent.snapshot.params.id;
    this.getQueue();
  }

  getQueue() {
    this.jobQueueService.getJobQueue(this.projectId, this.queueId)
      .pipe(tap(results => {
        if (results.jobTasksStatus) {
          results.jobTasksStatus.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
        }
      }))
      .subscribe(data => {
        this.job = data;

        if (!this.listened && (data.status === JobStatus.Queued || data.status === JobStatus.Processing)) {
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
