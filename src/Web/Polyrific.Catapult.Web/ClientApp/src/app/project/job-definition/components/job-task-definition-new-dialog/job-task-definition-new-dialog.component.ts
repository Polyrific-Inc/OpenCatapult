import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { JobDefinitionService, JobTaskDefinitionDto, ExternalServiceService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UtilityService } from '@app/shared/services/utility.service';

export interface NewJobDefinitionDialogData {
  projectId: number;
  jobDefinitionId: number;
  jobDefinitionName: string;
  isDeletion: boolean;
}


@Component({
  selector: 'app-job-task-definition-new-dialog',
  templateUrl: './job-task-definition-new-dialog.component.html',
  styleUrls: ['./job-task-definition-new-dialog.component.css']
})
export class JobTaskDefinitionNewDialogComponent implements OnInit {
  jobTaskDefinitionForm: FormGroup = new FormGroup({});
  loading: boolean;

  constructor (
    private jobDefinitionService: JobDefinitionService,
    private externalServiceService: ExternalServiceService,
    private snackbar: SnackbarService,
    private utilityService: UtilityService,
    public dialogRef: MatDialogRef<JobTaskDefinitionNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewJobDefinitionDialogData
    ) {
    }

  ngOnInit() {
    this.externalServiceService.getExternalServices().subscribe();
  }

  onFormReady(form: FormGroup) {
    this.jobTaskDefinitionForm = form;
  }

  onSubmit() {
    if (this.jobTaskDefinitionForm.valid) {
      this.loading = true;
      this.jobDefinitionService.createJobTaskDefinition(this.data.projectId, this.data.jobDefinitionId, this.jobTaskDefinitionForm.value)
        .subscribe(
            (data: JobTaskDefinitionDto) => {
              this.loading = false;
              this.snackbar.open('New job task definition has been created');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    } else {
      this.utilityService.markControlsAsTouched(this.jobTaskDefinitionForm);
    }
  }

}
