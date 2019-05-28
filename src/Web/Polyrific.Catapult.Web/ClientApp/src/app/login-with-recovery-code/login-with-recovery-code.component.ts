import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-with-recovery-code',
  templateUrl: './login-with-recovery-code.component.html',
  styleUrls: ['./login-with-recovery-code.component.css']
})
export class LoginWithRecoveryCodeComponent implements OnInit {
  loading = false;
  returnUrl: string;
  recoveryForm = this.fb.group({
    recoveryCode: [null, Validators.required]
  });
  error = '';
  private formSubmitAttempt: boolean;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {
    if (this.authService.currentUserValue) {
        this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onSubmit() {
    if (this.recoveryForm.valid) {
        this.loading = true;
      this.authService.login(this.recoveryForm.value)
        .subscribe(
            data => {
              this.router.navigate([this.returnUrl]);
            },
            (err: HttpErrorResponse) => {
                if (err.error && typeof err.error === 'string') {
                  this.error = err.error;
                } else {
                  this.error = err.message;
                }
                this.loading = false;
            });
    }

    this.formSubmitAttempt = true;
  }

  isFieldInvalid(field: string, error: string) {
    return (
      (!this.recoveryForm.get(field).hasError(error) && this.recoveryForm.get(field).touched) ||
      (this.recoveryForm.get(field).untouched && this.formSubmitAttempt)
    );
  }

}
