import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobDefinitionService, JobDefinitionDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NewModelDialogData } from '@app/project/data-model/components/data-model-new-dialog/data-model-new-dialog.component';

export interface NewJobDefinitionDialogData {
  projectId: number;
}

@Component({
  selector: 'app-job-definition-new-dialog',
  templateUrl: './job-definition-new-dialog.component.html',
  styleUrls: ['./job-definition-new-dialog.component.css']
})
export class JobDefinitionNewDialogComponent implements OnInit {
  jobDefinitionForm: FormGroup = this.fb.group({
    isDeletion: false
  });
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private jobDefinitionService: JobDefinitionService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<JobDefinitionNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewModelDialogData
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.jobDefinitionForm = this.fb.group({
      ...this.jobDefinitionForm.controls,
      ...form.controls
    });
  }

  onSubmit() {
    if (this.jobDefinitionForm.valid) {
      this.loading = true;
      this.jobDefinitionService.createJobDefinition(this.data.projectId, this.jobDefinitionForm.value)
        .subscribe(
            (data: JobDefinitionDto) => {
              this.loading = false;
              this.snackbar.open('New job definition has been created');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

}
