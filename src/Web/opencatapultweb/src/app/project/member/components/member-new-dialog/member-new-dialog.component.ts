import { Component, OnInit, Inject } from '@angular/core';
import { ProjectMemberDto, NewProjectMemberDto, projectMemberRoles } from '@app/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MemberService } from '@app/core/services/member.service';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface NewMemberViewModel {
  projectId: number;
}

@Component({
  selector: 'app-member-new-dialog',
  templateUrl: './member-new-dialog.component.html',
  styleUrls: ['./member-new-dialog.component.css']
})
export class MemberNewDialogComponent implements OnInit {
  newMemberForm: FormGroup;
  loading: boolean;
  projectMemberRoles = projectMemberRoles;

  constructor (
    private fb: FormBuilder,
    private memberService: MemberService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<MemberNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewMemberViewModel
    ) {
    }

  ngOnInit() {
    this.newMemberForm = this.fb.group({
      email: [null, Validators.required],
      firstName: null,
      lastName: null,
      projectMemberRoleId: [null, Validators.required]
    });
  }

  onFormReady(form: FormGroup) {
    this.newMemberForm = form;
  }

  onSubmit() {
    if (this.newMemberForm.valid) {
      this.loading = true;
      this.memberService.createMember(this.data.projectId, this.newMemberForm.value)
        .subscribe(
            (data: ProjectMemberDto) => {
              this.loading = false;
              this.snackbar.open('New project member has been created');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.newMemberForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
