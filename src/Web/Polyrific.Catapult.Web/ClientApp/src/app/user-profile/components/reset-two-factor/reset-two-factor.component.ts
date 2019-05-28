import { Component, OnInit } from '@angular/core';
import { SnackbarService } from '@app/shared';
import { AccountService } from '@app/core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-two-factor',
  templateUrl: './reset-two-factor.component.html',
  styleUrls: ['./reset-two-factor.component.css']
})
export class ResetTwoFactorComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private snackbar: SnackbarService,
    private router: Router) { }

  ngOnInit() {
  }

  onResetClick() {
    this.accountService.resetAuthenticatorKey()
      .subscribe(() => {
        this.router.navigate(['/user-profile/twofactor/enable']);
        this.snackbar.open('Your authenticator app key has been reset,' +
          'you will need to configure your authenticator app using the new key.', null, { duration: 5000});
      });
  }

}
