import { Component, OnInit, Inject, Input } from '@angular/core';
import { HelpContextDto } from '@app/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

interface HelpContextViewModel {
  item: HelpContextDto;
  last: boolean;
}

@Component({
  selector: 'app-help-context-dialog',
  templateUrl: './help-context-dialog.component.html',
  styleUrls: ['./help-context-dialog.component.css']
})
export class HelpContextDialogComponent implements OnInit {
  constructor (
    public dialogRef: MatDialogRef<HelpContextDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: HelpContextViewModel
    ) {
    }

  ngOnInit() {
  }

  getTitle(): string {
    if (this.data && this.data.item) {
      return `: ${this.data.item.subSection ? this.data.item.subSection : this.data.item.section}`;
    }

    return '';
  }
}
