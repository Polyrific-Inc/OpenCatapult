import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray } from '@angular/forms';
import { ExternalServiceService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef } from '@angular/material';
import { GenericService } from '@app/external-service/services/generic.service';
import { ExternalServiceFormOutput } from '../external-service-form/external-service-form.component';

@Component({
  selector: 'app-external-service-new-dialog',
  templateUrl: './external-service-new-dialog.component.html',
  styleUrls: ['./external-service-new-dialog.component.css']
})
export class ExternalServiceNewDialogComponent implements OnInit {
  externalServiceForm: FormGroup = new FormGroup({});
  genericConfigForm: FormArray;
  loading: boolean;

  constructor (
    private externalServiceService: ExternalServiceService,
    private snackbar: SnackbarService,
    private genericService: GenericService,
    public dialogRef: MatDialogRef<ExternalServiceNewDialogComponent>
    ) {
    }

  ngOnInit() {
  }

  onFormReady(data: ExternalServiceFormOutput) {
    this.externalServiceForm = data.mainForm;
    this.genericConfigForm = data.genericConfigForm;
  }

  onSubmit() {
    if (this.externalServiceForm.valid && this.genericConfigForm.valid) {
      this.loading = true;
      this.externalServiceService.createExternalService(this.genericService.getFormValue(this.externalServiceForm, this.genericConfigForm))
        .subscribe(
            (data: ExternalServiceService) => {
              this.loading = false;
              this.snackbar.open('New external service has been created');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

}
