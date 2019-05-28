import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';
import { Router } from '@angular/router';

@Component({
  selector: 'app-disable-two-factor',
  templateUrl: './disable-two-factor.component.html',
  styleUrls: ['./disable-two-factor.component.css']
})
export class DisableTwoFactorComponent implements OnInit {

  constructor(
    private accountService: AccountService,
    private snackbar: SnackbarService,
    private router: Router) { }

  ngOnInit() {
  }

  onDisableClick() {
    this.accountService.disableTwoFactor()
      .subscribe(() => {
        this.router.navigate(['/user-profile/twofactor']);
        this.snackbar.open('2fa has been disabled. You can reenable 2fa when you setup an authenticator app',
         null, { duration: 5000});
      });
  }

}
