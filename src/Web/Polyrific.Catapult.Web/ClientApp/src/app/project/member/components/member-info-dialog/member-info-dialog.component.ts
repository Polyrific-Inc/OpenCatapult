import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MemberService } from '@app/core/services/member.service';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProjectMemberDto, projectMemberRoles } from '@app/core';

@Component({
  selector: 'app-member-info-dialog',
  templateUrl: './member-info-dialog.component.html',
  styleUrls: ['./member-info-dialog.component.css']
})
export class MemberInfoDialogComponent implements OnInit {
  memberInfoForm: FormGroup = this.fb.group({
    id: [{value: null, disabled: true}],
    username: [{value: null, disabled: true}],
    projectMemberRoleId: [{value: null, disabled: true}, Validators.required],
  });
  editing: boolean;
  loading: boolean;
  projectMemberRoles = projectMemberRoles;

  constructor (
    private fb: FormBuilder,
    private memberService: MemberService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<MemberInfoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public member: ProjectMemberDto
    ) {
    }

  ngOnInit() {
    this.memberInfoForm.patchValue(this.member);
  }

  onSubmit() {
    if (this.memberInfoForm.valid) {
      this.loading = true;
      this.memberService.updateMember(this.member.projectId, this.member.id,
        { id: this.member.id, userId: this.member.userId, projectMemberRoleId: this.memberInfoForm.get('projectMemberRoleId').value })
        .subscribe(
            () => {
              this.loading = false;
              this.snackbar.open('Project Member has been updated');
              this.dialogRef.close(true);
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.memberInfoForm.get('projectMemberRoleId').enable();
    this.editing = true;
  }

  onCancelClick() {
    this.memberInfoForm.get('projectMemberRoleId').disable();
    this.editing = false;
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.memberInfoForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }
}
