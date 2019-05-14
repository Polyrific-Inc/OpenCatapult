import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { JobQueueService, JobQueueDto, JobStatus } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-job-queue-cancel-dialog',
  templateUrl: './job-queue-cancel-dialog.component.html',
  styleUrls: ['./job-queue-cancel-dialog.component.css']
})
export class JobQueueCancelDialogComponent implements OnInit {
  jobQueueCancelForm: FormGroup = this.fb.group({
    remarks: null
  });
  loading: boolean;

  constructor (
    private jobQueueService: JobQueueService,
    private fb: FormBuilder,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<JobQueueCancelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: JobQueueDto
    ) {
    }

  ngOnInit() {
  }

  onSubmit() {
    if (this.jobQueueCancelForm.valid) {
      this.loading = true;
      this.jobQueueService.updateJobQueue(this.data.projectId, this.data.id, {
        ...this.data,
        status: JobStatus.Cancelled,
        ...this.jobQueueCancelForm.value,
      })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('The job queue has been cancelled');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

}
