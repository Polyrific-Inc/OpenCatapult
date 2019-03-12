import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JobQueueService, JobDto, JobStatus, ProjectService } from '@app/core';
import { tap } from 'rxjs/operators';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
import { MatDialog } from '@angular/material';
import { JobQueueCancelDialogComponent } from '../components/job-queue-cancel-dialog/job-queue-cancel-dialog.component';

@Component({
  selector: 'app-job-queue-detail',
  templateUrl: './job-queue-detail.component.html',
  styleUrls: ['./job-queue-detail.component.css']
})
export class JobQueueDetailComponent implements OnInit {
  queueId: number;
  projectId: number;
  job: JobDto;
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

      this.allowRestart = data.jobQueue.status === JobStatus.Cancelled || data.jobQueue.status === JobStatus.Pending ||
        data.jobQueue.status === JobStatus.Error;
      this.allowCancel = data.jobQueue.status === JobStatus.Processing || data.jobQueue.status === JobStatus.Pending;
      this.allowRefresh = data.jobQueue.status !== JobStatus.Completed;
    });
  }

  onCancelClick() {
    const dialogRef = this.dialog.open(JobQueueCancelDialogComponent, {
      data: this.job
    });

    dialogRef.afterClosed().subscribe((success) => {
      if (success) {
        this.getQueue();
      }
    });
  }

  onRestartClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Restart Queue',
        confirmationText: `Are you sure you want to restart the queue '${this.job.code}'?`
      }
    });

    const self = this;
    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.jobQueueService.updateJobQueue(self.projectId, self.queueId, {
          ...self.job,
          status: JobStatus.Queued,
          remarks: null,
        }).subscribe(() => {
          this.getQueue();
          this.snackbar.open('Job has been restarted');
        },
        err => {
          this.snackbar.open(err);
        });
      }
    });
  }

}
