import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DataModelService, DataModelDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UtilityService } from '@app/shared/services/utility.service';

@Component({
  selector: 'app-data-model-info-dialog',
  templateUrl: './data-model-info-dialog.component.html',
  styleUrls: ['./data-model-info-dialog.component.css']
})
export class DataModelInfoDialogComponent implements OnInit {
  dataModelForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  editing: boolean;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private dataModelService: DataModelService,
    private snackbar: SnackbarService,
    private utilityService: UtilityService,
    public dialogRef: MatDialogRef<DataModelInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataModel: DataModelDto
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.dataModelForm = this.fb.group({
      ...this.dataModelForm.controls,
      ...form.controls
    });
    this.dataModelForm.patchValue(this.dataModel);
  }

  onSubmit() {
    if (this.dataModelForm.valid) {
      this.loading = true;
      this.dataModelService.updateDataModel(this.dataModel.projectId, this.dataModel.id,
        { id: this.dataModel.id, ...this.dataModelForm.value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('Data model has been updated');
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

  onEditClick() {
    this.editing = true;
  }

  onCancelClick() {
    this.utilityService.markControlsAsTouched(this.dataModelForm, false);
    this.editing = false;
  }

}
