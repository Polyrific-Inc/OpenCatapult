import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JobQueueService, JobQueueDto, JobStatus } from '@app/core';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { MatDialog } from '@angular/material';
import { JobQueueCancelDialogComponent } from '../components/job-queue-cancel-dialog/job-queue-cancel-dialog.component';

@Component({
  selector: 'app-job-queue-detail',
  templateUrl: './job-queue-detail.component.html',
  styleUrls: ['./job-queue-detail.component.css']
})
export class JobQueueDetailComponent implements OnInit {
  jobQueueId: number;
  projectId: number;
  jobQueue: JobQueueDto;
  allowRestart: boolean;
  allowCancel: boolean;
  allowRefresh: boolean;
  constructor(
    private route: ActivatedRoute,
    private jobQueueService: JobQueueService,
    private snackbar: SnackbarService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.route.data.subscribe((data: {jobQueue: JobQueueDto}) => {
      this.initializeViewData(data.jobQueue);
    });
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
    this.allowRefresh = jobQueue.status !== JobStatus.Completed;
  }

  getJobQueue() {
    this.jobQueueService.getJobQueue(this.projectId, this.jobQueueId)
      .subscribe(data => this.initializeViewData(data));
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
