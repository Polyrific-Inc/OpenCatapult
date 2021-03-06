import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../core/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loading = false;
  returnUrl: string;
  error = '';
  loginForm = this.fb.group({
    userName: [null, Validators.required],
    password: [null, Validators.required]
  });

  private formSubmitAttempt: boolean;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService) {
      if (this.authService.currentUserValue) {
          this.router.navigate(['/']);
      }
    }

  ngOnInit() {
    // get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  isFieldInvalid(field: string, error: string) {
    return (
      (!this.loginForm.get(field).hasError(error) && this.loginForm.get(field).touched) ||
      (this.loginForm.get(field).untouched && this.formSubmitAttempt)
    );
  }

  onSubmit() {
    if (this.loginForm.valid) {
        this.loading = true;
      this.authService.login(this.loginForm.value)
        .subscribe(
            response => {
                if (response === 'Requires two factor') {
                  this.router.navigate(['/login-with-2fa'], { queryParams: { returnUrl: this.returnUrl }});
                } else {
                  this.router.navigate([this.returnUrl]);
                }
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

}
