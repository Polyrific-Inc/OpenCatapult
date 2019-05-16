import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DataModelService, DataModelPropertyDto, DataModelDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UtilityService } from '@app/shared/services/utility.service';

export interface DataModelPropertyViewModel extends DataModelPropertyDto {
  projectId: number;
  dataModelName: string;
  relatedDataModels: DataModelDto[];
}

@Component({
  selector: 'app-data-model-property-info-dialog',
  templateUrl: './data-model-property-info-dialog.component.html',
  styleUrls: ['./data-model-property-info-dialog.component.css']
})
export class DataModelPropertyInfoDialogComponent implements OnInit {
  dataModelPropertyForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  editing: boolean;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private dataModelService: DataModelService,
    private snackbar: SnackbarService,
    private utilityService: UtilityService,
    public dialogRef: MatDialogRef<DataModelPropertyInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataModelProperty: DataModelPropertyViewModel
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.dataModelPropertyForm = this.fb.group({
      ...this.dataModelPropertyForm.controls,
      ...form.controls
    });
    this.dataModelPropertyForm.patchValue(this.dataModelProperty);
  }

  onSubmit() {
    if (this.dataModelPropertyForm.valid) {
      this.loading = true;
      this.dataModelService.updateDataModelProperty(
        this.dataModelProperty.projectId,
        this.dataModelProperty.projectDataModelId,
        { id: this.dataModelProperty.id, ...this.dataModelPropertyForm.value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('Data model property has been updated');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    } else {
      this.utilityService.markControlsAsTouched(this.dataModelPropertyForm);
    }
  }

  onEditClick() {
    this.editing = true;
  }

  onCancelClick() {
    this.utilityService.markControlsAsTouched(this.dataModelPropertyForm, false);
    this.editing = false;
  }

}
