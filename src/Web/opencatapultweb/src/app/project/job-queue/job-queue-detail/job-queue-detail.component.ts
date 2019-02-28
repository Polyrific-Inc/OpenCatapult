import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JobQueueService, JobDto, JobStatus } from '@app/core';
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
        this.allowRestart = data.status === JobStatus.Cancelled || data.status === JobStatus.Pending || data.status === JobStatus.Error;
        this.allowCancel = data.status === JobStatus.Processing || data.status === JobStatus.Pending;
        this.allowRefresh = data.status !== JobStatus.Completed;
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
