import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/core';
import { User2faInfoDto } from '@app/core/models/account/user-2fa-info-dto';

@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrls: ['./two-factor-auth.component.css']
})
export class TwoFactorAuthComponent implements OnInit {
  loading: boolean;
  user2faInfo: User2faInfoDto;

  constructor(
    private accountService: AccountService) { }

  ngOnInit() {
    this.loading = true;
    this.accountService.getUser2faInfo()
      .subscribe(data => {
        this.loading = false;
        this.user2faInfo = data;
      });
  }

}
