import { Component, OnInit, Inject } from '@angular/core';
import { DataModelPropertyDto, DataModelService, DataModelDto } from '@app/core';
import { FormGroup } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UtilityService } from '@app/shared/services/utility.service';

export interface NewModelPropertyDialogData {
  projectId: number;
  modelId: number;
  modelName: string;
  relatedDataModels: DataModelDto[];
}

@Component({
  selector: 'app-data-model-property-new-dialog',
  templateUrl: './data-model-property-new-dialog.component.html',
  styleUrls: ['./data-model-property-new-dialog.component.css']
})
export class DataModelPropertyNewDialogComponent implements OnInit {
  dataModelPropertyForm: FormGroup = new FormGroup({});
  loading: boolean;

  constructor (
    private dataModelService: DataModelService,
    private snackbar: SnackbarService,
    private utilityService: UtilityService,
    public dialogRef: MatDialogRef<DataModelPropertyNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewModelPropertyDialogData
    ) {
    }

  ngOnInit() {
  }

  onFormReady(form: FormGroup) {
    this.dataModelPropertyForm = form;
  }

  onSubmit() {
    if (this.dataModelPropertyForm.valid) {
      this.loading = true;
      this.dataModelService.createDataModelProperty(this.data.projectId, this.data.modelId, this.dataModelPropertyForm.value)
        .subscribe(
            (data: DataModelPropertyDto) => {
              this.loading = false;
              this.snackbar.open('New data model has been created');
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


}
