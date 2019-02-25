import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface DialogData {
  title: string;
  confirmationText: string;
  subText: string;
  confirmationMatch: string;
  enteredConfirmation: string;
}

@Component({
  templateUrl: './confirmation-with-input-dialog.component.html',
  styleUrls: ['./confirmation-with-input-dialog.component.css']
})
export class ConfirmationWithInputDialogComponent {
  inputControl: FormControl;

  constructor(
    public dialogRef: MatDialogRef<ConfirmationWithInputDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
    ) {
      this.inputControl =  new FormControl(data.enteredConfirmation,
        Validators.compose([Validators.required, Validators.pattern(data.confirmationMatch)]));
    }

    onCancelClick() {
      this.dialogRef.close();
    }

    onOkClick() {
      if (this.inputControl.valid) {
        this.dialogRef.close(true);
      }
    }

}
