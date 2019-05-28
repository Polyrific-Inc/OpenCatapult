import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/core';
import { SnackbarService } from '@app/shared';

@Component({
  selector: 'app-reset-recovery-two-factor',
  templateUrl: './reset-recovery-two-factor.component.html',
  styleUrls: ['./reset-recovery-two-factor.component.css']
})
export class ResetRecoveryTwoFactorComponent implements OnInit {
  loading: boolean;
  recoveryCodes: string[];

  constructor(private accountService: AccountService, private snackbar: SnackbarService) { }

  ngOnInit() {
  }

  onGenerateRecoveryCodesClick() {
    this.loading = true;
    this.accountService.generate2faRecoveryCodes()
      .subscribe(data => {
        this.loading = false;
        this.snackbar.open('You have generated new recovery codes.');
        this.recoveryCodes = data.recoveryCodes;
      });
  }

}
