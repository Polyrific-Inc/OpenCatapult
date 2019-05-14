import { Component, OnInit, Inject } from '@angular/core';
import { ExternalServiceService, ExternalServiceDto } from '@app/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ExternalServiceFormOutput } from '../external-service-form/external-service-form.component';
import { GenericService } from '@app/external-service/services/generic.service';

@Component({
  selector: 'app-external-service-info-dialog',
  templateUrl: './external-service-info-dialog.component.html',
  styleUrls: ['./external-service-info-dialog.component.css']
})
export class ExternalServiceInfoDialogComponent implements OnInit {
  externalServiceForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  genericConfigForm: FormArray;
  editing: boolean;
  loading: boolean;
  externalServiceWithProp: ExternalServiceDto;

  constructor (
    private fb: FormBuilder,
    private externalServiceService: ExternalServiceService,
    private snackbar: SnackbarService,
    private genericService: GenericService,
    public dialogRef: MatDialogRef<ExternalServiceInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public externalService: ExternalServiceDto
    ) {
    }

  ngOnInit() {
  }

  onFormReady(formData: ExternalServiceFormOutput) {
    this.externalServiceForm = formData.mainForm;
    this.externalServiceForm.setControl('id', this.fb.control({value: null, disabled: true}));

    this.genericConfigForm = formData.genericConfigForm;
    this.externalServiceService.getExternalService(this.externalService.id)
      .subscribe(data => {
        this.externalServiceWithProp = data;
        this.genericService.patchFormValue(this.externalServiceForm, formData.genericConfigForm, data);
      });
  }

  onSubmit() {
    if (this.externalServiceForm.valid && this.genericConfigForm.valid) {
      this.loading = true;
      this.externalServiceService.updateExternalService(this.externalService.id,
        { id: this.externalService.id, ...this.genericService.getFormValue(this.externalServiceForm, this.genericConfigForm) })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('External Service has been updated');
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
    this.genericConfigForm.enable();
  }

  onCancelClick() {
    this.editing = false;
    this.genericService.patchFormValue(this.externalServiceForm, this.genericConfigForm, this.externalServiceWithProp);
    this.genericConfigForm.disable();
  }

}
