import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobDefinitionService, JobDefinitionDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-job-definition-info-dialog',
  templateUrl: './job-definition-info-dialog.component.html',
  styleUrls: ['./job-definition-info-dialog.component.css']
})
export class JobDefinitionInfoDialogComponent implements OnInit {
  jobDefinitionForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  editing: boolean;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private jobDefinitionService: JobDefinitionService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<JobDefinitionInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public jobDefinition: JobDefinitionDto
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.jobDefinitionForm = this.fb.group({
      ...this.jobDefinitionForm.controls,
      ...form.controls
    });
    this.jobDefinitionForm.patchValue(this.jobDefinition);
  }

  onSubmit() {
    if (this.jobDefinitionForm.valid) {
      this.loading = true;
      this.jobDefinitionService.updateJobDefinition(this.jobDefinition.projectId, this.jobDefinition.id,
        { id: this.jobDefinition.id, ...this.jobDefinitionForm.value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('Job Definition has been updated');
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
