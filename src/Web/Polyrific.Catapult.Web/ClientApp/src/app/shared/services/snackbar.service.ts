import { Injectable } from '@angular/core';
import { MatSnackBarConfig, MatSnackBar } from '@angular/material';

@Injectable()
export class SnackbarService {

  constructor(private snackbar: MatSnackBar) { }

  open(message: string, action?: string, config?: MatSnackBarConfig<any>) {
    config = config || {};
    config.duration = config.duration || 2000;

    this.snackbar.open(message, action, config);
  }
}
