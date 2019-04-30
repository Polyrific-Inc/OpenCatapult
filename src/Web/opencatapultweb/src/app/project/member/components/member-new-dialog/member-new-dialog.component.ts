import { Component, OnInit, Inject } from '@angular/core';
import { ProjectMemberDto, projectMemberRoles, AccountService, UserDto } from '@app/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MemberService } from '@app/core/services/member.service';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { TextHelperService } from '@app/core/services/text-helper.service';

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
  userNameControl: FormControl;
  firstNameControl: FormControl;
  lastNameControl: FormControl;
  loading: boolean;
  projectMemberRoles = projectMemberRoles;
  newUser: boolean;
  currentUser: UserDto;
  private userName = new Subject<string>();

  constructor (
    private fb: FormBuilder,
    private memberService: MemberService,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    private textHelperService: TextHelperService,
    public dialogRef: MatDialogRef<MemberNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewMemberViewModel
    ) {
    }

  ngOnInit() {
    this.userNameControl = this.fb.control(null, Validators.required);
    this.firstNameControl = this.fb.control({value: null, disabled: true});
    this.lastNameControl = this.fb.control({value: null, disabled: true});
    this.newMemberForm = this.fb.group({
      userName: this.userNameControl,
      firstName: this.firstNameControl,
      lastName: this.lastNameControl,
      projectMemberRoleId: [null, Validators.required]
    });

    this.getUserByUserName();
  }

  onFormReady(form: FormGroup) {
    this.newMemberForm = form;
  }

  onSubmit() {
    if (this.newMemberForm.valid) {
      this.loading = true;
      this.memberService.createMember(this.data.projectId, {
        projectId: this.data.projectId,
        userId: this.currentUser ? this.currentUser.id : 0,
        email: this.currentUser ? null : this.newMemberForm.value.userName,
        ...this.newMemberForm.value
      })
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

  onUserNameChanged(userName: string) {
    if (this.userNameControl.valid) {
      this.userName.next(userName);
    }
  }

  getUserByUserName() {
    this.userName.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(userName => {
      this.loading = true;
      this.userNameControl.clearValidators();
      this.accountService.getUserByUserName(userName)
        .subscribe(user => {
          this.loading = false;
          if (user) {
            this.firstNameControl.disable();
            this.lastNameControl.disable();
            this.newMemberForm.patchValue(user);
            this.newUser = false;
            this.currentUser = user;
          } else {
            this.firstNameControl.setValue('');
            this.lastNameControl.setValue('');
            this.firstNameControl.enable();
            this.lastNameControl.enable();
            this.newUser = true;
            this.currentUser = null;

            if (!this.textHelperService.validateEmail(userName)) {
              this.userNameControl.setErrors({
                'newEmail': true
              });
            }
          }
        });
    });
  }

}
