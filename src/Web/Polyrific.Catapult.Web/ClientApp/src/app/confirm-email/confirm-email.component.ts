import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '@app/core';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  confirmResult: string;
  loading: boolean;
  confirmSuccess: boolean;
  constructor(
    private accountService: AccountService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    const userId = this.route.snapshot.queryParams['userId'];
    const token = this.route.snapshot.queryParams['token'];

    this.loading = true;
    this.accountService.confirmEmail(userId, encodeURIComponent(token))
      .subscribe(result => {
        this.confirmResult = result;
        this.confirmSuccess = true;
        this.loading = false;
      }, err => {
        this.confirmResult = err;
        this.loading = false;
      });
  }

}
