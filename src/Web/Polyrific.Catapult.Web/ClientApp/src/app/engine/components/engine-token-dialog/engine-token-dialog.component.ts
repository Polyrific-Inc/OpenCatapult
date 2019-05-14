import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { EngineService, EngineDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-engine-token-dialog',
  templateUrl: './engine-token-dialog.component.html',
  styleUrls: ['./engine-token-dialog.component.css']
})
export class EngineTokenDialogComponent implements OnInit {
  engineForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}]
  });
  engineToken: string;
  loading: boolean;

  constructor (
    private fb: FormBuilder,
    private engineService: EngineService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<EngineTokenDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public engine: EngineDto
    ) {
    }

  ngOnInit() {
    this.engineForm = this.fb.group({
      id: [{value: null, disabled: true}],
      name: [{value: null, disabled: true}],
      expires: null
    });
    this.engineForm.patchValue(this.engine);
  }

  onSubmit() {
    if (this.engineForm.valid) {
      this.loading = true;
      this.engineService.requestToken(this.engine.id, this.engineForm.value)
        .subscribe(
            (token) => {
              this.engineToken = token;
              this.loading = false;
              this.snackbar.open('Engine token has been generated');
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onCopyTokenClick(tokenElement) {
    window.getSelection().selectAllChildren(tokenElement);
    document.execCommand('copy');
    this.snackbar.open('Copied');
  }
}
