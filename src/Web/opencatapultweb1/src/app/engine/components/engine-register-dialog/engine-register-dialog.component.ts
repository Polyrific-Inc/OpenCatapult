import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EngineService, EngineDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-engine-register-dialog',
  templateUrl: './engine-register-dialog.component.html',
  styleUrls: ['./engine-register-dialog.component.css']
})
export class EngineRegisterDialogComponent implements OnInit {
  engineForm: FormGroup;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private engineService: EngineService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<EngineRegisterDialogComponent>
    ) {
    }

  ngOnInit() {
    this.engineForm = this.fb.group({
      name: [null, Validators.required]
    });
  }

  onSubmit() {
    if (this.engineForm.valid) {
      this.loading = true;
      this.engineService.register(this.engineForm.value)
        .subscribe(
            (data: EngineDto) => {
              this.loading = false;
              this.snackbar.open('New engine has been registered');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.engineForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
