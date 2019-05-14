import { Component, OnInit, Inject } from '@angular/core';
import { HelpContextDto } from '@app/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-help-context-dialog',
  templateUrl: './help-context-dialog.component.html',
  styleUrls: ['./help-context-dialog.component.css']
})
export class HelpContextDialogComponent implements OnInit {

  constructor (
    public dialogRef: MatDialogRef<HelpContextDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public helpContexts: HelpContextDto[]
    ) {
    }

  ngOnInit() {
  }

}
