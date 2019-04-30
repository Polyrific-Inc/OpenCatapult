import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TaskProviderService, TaskProviderDto, RegisterTaskProviderDto, YamlService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-task-provider-register-dialog',
  templateUrl: './task-provider-register-dialog.component.html',
  styleUrls: ['./task-provider-register-dialog.component.css']
})
export class TaskProviderRegisterDialogComponent implements OnInit {
  taskProviderForm: FormGroup;
  metadataFile: string;
  newTaskProvider: RegisterTaskProviderDto;
  loading: boolean;
  formSubmitted: boolean;

  constructor (
    private fb: FormBuilder,
    private taskProviderService: TaskProviderService,
    private snackbar: SnackbarService,
    private yamlService: YamlService,
    public dialogRef: MatDialogRef<TaskProviderRegisterDialogComponent>
    ) {
    }

  ngOnInit() {
    this.taskProviderForm = this.fb.group({
      metadataFile: [null, Validators.required],
      name: [{value: null, disabled: true}],
      type: [{value: null, disabled: true}],
      author: [{value: null, disabled: true}],
      version: [{value: null, disabled: true}]
    });
  }

  onSubmit() {
    this.formSubmitted = true;
    if (this.taskProviderForm.valid) {
      this.loading = true;
      this.taskProviderService.registerTaskProvider(this.newTaskProvider)
        .subscribe(
            (data: TaskProviderDto) => {
              this.loading = false;
              this.snackbar.open('New task provider has been registered');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.taskProviderForm.get(controlName);
    return this.formSubmitted && control.invalid && control.errors && control.getError(errorCode);
  }

  onFileChanged(event) {
    if (event.target.value) {
      this.metadataFile = event.target.value.split(/(\\|\/)/g).pop();

      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        this.newTaskProvider = this.yamlService.deserialize(fileReader.result);
        this.taskProviderForm.patchValue(this.newTaskProvider);
      };
      fileReader.readAsText(event.target.files[0]);
    }
  }

}
