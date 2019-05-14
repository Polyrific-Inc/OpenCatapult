import { Component, OnInit, OnDestroy } from '@angular/core';
import { JobQueueDto, JobQueueService } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { JobLogDto } from '@app/core/models/job-queue/job-log-dto';
import { JobStatus } from '@app/core/enums/job-status';
import { finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material';
import { JobQueueCancelDialogComponent } from '../components/job-queue-cancel-dialog/job-queue-cancel-dialog.component';
import { ConfirmationDialogComponent, SnackbarService } from '@app/shared';

@Component({
  selector: 'app-job-queue-log',
  templateUrl: './job-queue-log.component.html',
  styleUrls: ['./job-queue-log.component.css']
})
export class JobQueueLogComponent implements OnInit, OnDestroy {
  jobQueueId: number;
  projectId: number;
  jobQueue: JobQueueDto;
  log$: Observable<JobLogDto>;
  listened: boolean;
  logReceived: boolean;
  allowRestart: boolean;
  allowCancel: boolean;
  constructor(
    private route: ActivatedRoute,
    private jobQueueService: JobQueueService,
    private dialog: MatDialog,
    private snackbar: SnackbarService
  ) { }

  ngOnInit() {
    this.route.data.subscribe((data: {jobQueue: JobQueueDto}) => {
      this.initializeViewData(data.jobQueue);
    });
  }

  ngOnDestroy() {
    this.jobQueueService.disconnectJobQueueLog();
  }

  initializeViewData(jobQueue: JobQueueDto) {
    if (jobQueue.jobTasksStatus) {
      jobQueue.jobTasksStatus.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
    }

    this.jobQueue = jobQueue;
    this.jobQueueId = this.jobQueue.id;
    this.projectId = this.jobQueue.projectId;

    this.allowRestart = jobQueue.status === JobStatus.Cancelled || jobQueue.status === JobStatus.Pending ||
      jobQueue.status === JobStatus.Error;
    this.allowCancel = jobQueue.status === JobStatus.Processing || jobQueue.status === JobStatus.Pending;

    if (!this.listened && (jobQueue.status === JobStatus.Queued || jobQueue.status === JobStatus.Processing)) {
      this.listenQueueLog();
    }
  }

  getJobQueue() {
    this.jobQueueService.getJobQueue(this.projectId, this.jobQueueId).subscribe(data => this.initializeViewData(data));
  }

  listenQueueLog() {
    this.log$ = this.jobQueueService.listenJobQueueLog(this.projectId, this.jobQueueId);
    let previousTask = '';
    this.log$
      .pipe(finalize(() => this.getJobQueue()))
      .subscribe((log) => {
      if (previousTask !== log.taskName) {
        previousTask = log.taskName;
        this.getJobQueue();
      }
    });
    this.listened = true;
  }

  onCancelClick() {
    const dialogRef = this.dialog.open(JobQueueCancelDialogComponent, {
      data: this.jobQueue
    });

    dialogRef.afterClosed().subscribe((success) => {
      if (success) {
        this.getJobQueue();
      }
    });
  }

  onRestartClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Restart Queue',
        confirmationText: `Are you sure you want to restart the queue '${this.jobQueue.code}'?`
      }
    });

    const self = this;
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.jobQueueService.updateJobQueue(self.projectId, self.jobQueueId, {
          ...self.jobQueue,
          status: JobStatus.Queued,
          remarks: null,
        }).subscribe(() => {
          this.getJobQueue();
          this.snackbar.open('Job has been restarted');
        },
        err => {
          this.snackbar.open(err);
        });
      }
    });
  }

}
