import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MemberRoutingModule } from './member-routing.module';
import { MemberComponent } from './member/member.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatTableModule, MatIconModule, MatDialogModule, MatInputModule,
  MatSelectModule, MatProgressBarModule, MatButtonModule } from '@angular/material';
import { SharedModule } from '@app/shared/shared.module';
import { MemberInfoDialogComponent } from './components/member-info-dialog/member-info-dialog.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MemberNewDialogComponent } from './components/member-new-dialog/member-new-dialog.component';

@NgModule({
  declarations: [MemberComponent, MemberInfoDialogComponent, MemberNewDialogComponent],
  imports: [
    CommonModule,
    MemberRoutingModule,
    FlexLayoutModule,
    MatTableModule,
    MatIconModule,
    SharedModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatProgressBarModule,
    MatButtonModule,
    MatSelectModule
  ],
  entryComponents: [ MemberNewDialogComponent, MemberInfoDialogComponent ]
})
export class MemberModule { }
