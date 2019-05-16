import { Component, OnInit, Input, Inject } from '@angular/core';
import { DataModelDto, DataModelService } from '@app/core';
import { FormGroup } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { UtilityService } from '@app/shared/services/utility.service';

export interface NewModelDialogData {
  projectId: number;
}

@Component({
  selector: 'app-data-model-new-dialog',
  templateUrl: './data-model-new-dialog.component.html',
  styleUrls: ['./data-model-new-dialog.component.css']
})
export class DataModelNewDialogComponent implements OnInit {
  dataModelForm: FormGroup = new FormGroup({});
  loading: boolean;

  constructor (
    private dataModelService: DataModelService,
    private snackbar: SnackbarService,
    private utilityService: UtilityService,
    public dialogRef: MatDialogRef<DataModelNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewModelDialogData
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.dataModelForm = form;
  }

  onSubmit() {
    if (this.dataModelForm.valid) {
      this.loading = true;
      this.dataModelService.createDataModel(this.data.projectId, this.dataModelForm.value)
        .subscribe(
            (data: DataModelDto) => {
              this.loading = false;
              this.snackbar.open('New data model has been created');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    } else {
      this.utilityService.markControlsAsTouched(this.dataModelForm);
    }
  }

}
