import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/core/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-with-twofa',
  templateUrl: './login-with-twofa.component.html',
  styleUrls: ['./login-with-twofa.component.css']
})
export class LoginWithTwofaComponent implements OnInit {
  loading = false;
  returnUrl: string;
  authenticatorForm = this.fb.group({
    authenticatorCode: [null, Validators.required]
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
    if (this.authenticatorForm.valid) {
        this.loading = true;
      this.authService.login(this.authenticatorForm.value)
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
      (!this.authenticatorForm.get(field).hasError(error) && this.authenticatorForm.get(field).touched) ||
      (this.authenticatorForm.get(field).untouched && this.formSubmitAttempt)
    );
  }

}
