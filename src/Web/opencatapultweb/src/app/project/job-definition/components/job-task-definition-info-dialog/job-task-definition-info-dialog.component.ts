import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobDefinitionService, JobTaskDefinitionDto, ExternalServiceService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface JobTaskDefinitionViewModel extends JobTaskDefinitionDto {
  projectId: number;
  jobDefinitionName: string;
}

@Component({
  selector: 'app-job-task-definition-info-dialog',
  templateUrl: './job-task-definition-info-dialog.component.html',
  styleUrls: ['./job-task-definition-info-dialog.component.css']
})
export class JobTaskDefinitionInfoDialogComponent implements OnInit {
  jobTaskDefinitionForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  jobTaskDefinitionInfoForm: FormGroup;
  editing: boolean;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private jobDefinitionService: JobDefinitionService,
    private externalServiceService: ExternalServiceService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<JobTaskDefinitionInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public jobTaskDefinition: JobTaskDefinitionViewModel
    ) {
    }

  ngOnInit() {
    this.externalServiceService.getExternalServices().subscribe();
  }

  onFormReady(form: FormGroup) {
    this.jobTaskDefinitionInfoForm = form;
    this.jobTaskDefinitionForm.patchValue(this.jobTaskDefinition);
    this.jobTaskDefinitionInfoForm.patchValue(this.jobTaskDefinition);
  }

  onSubmit() {
    if (this.jobTaskDefinitionInfoForm.valid) {
      this.loading = true;
      this.jobDefinitionService.updateJobTaskDefinition(this.jobTaskDefinition.projectId,
        this.jobTaskDefinition.jobDefinitionId, this.jobTaskDefinition.id,
        { id: this.jobTaskDefinition.id, ...this.jobTaskDefinitionInfoForm.value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('Job Task Definition has been updated');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.editing = true;
  }

  onCancelClick() {
    this.editing = false;
  }
}
