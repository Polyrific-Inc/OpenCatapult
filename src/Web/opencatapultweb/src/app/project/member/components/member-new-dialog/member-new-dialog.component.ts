import { Component, OnInit, Inject } from '@angular/core';
import { ProjectMemberDto, projectMemberRoles, AccountService, UserDto } from '@app/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MemberService } from '@app/core/services/member.service';
import { SnackbarService } from '@app/shared';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

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
  emailControl: FormControl;
  firstNameControl: FormControl;
  lastNameControl: FormControl;
  loading: boolean;
  projectMemberRoles = projectMemberRoles;
  newUser: boolean;
  currentUser: UserDto;
  private email = new Subject<string>();

  constructor (
    private fb: FormBuilder,
    private memberService: MemberService,
    private accountService: AccountService,
    private snackbar: SnackbarService,
    public dialogRef: MatDialogRef<MemberNewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewMemberViewModel
    ) {
    }

  ngOnInit() {
    this.emailControl = this.fb.control(null, Validators.compose([Validators.required, Validators.email]));
    this.firstNameControl = this.fb.control({value: null, disabled: true});
    this.lastNameControl = this.fb.control({value: null, disabled: true});
    this.newMemberForm = this.fb.group({
      email: this.emailControl,
      firstName: this.firstNameControl,
      lastName: this.lastNameControl,
      projectMemberRoleId: [null, Validators.required]
    });

    this.getUserByEmail();
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
        userName: this.newMemberForm.value.email,
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

  onEmailChanged(email: string) {
    if (this.emailControl.valid) {
      this.email.next(email);
    }
  }

  getUserByEmail() {
    this.email.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(email => {
      this.loading = true;
      this.accountService.getUserByEmail(email)
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
          }
        });
    });
  }

}
