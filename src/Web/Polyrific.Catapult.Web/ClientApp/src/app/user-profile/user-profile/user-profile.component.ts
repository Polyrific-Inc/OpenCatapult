import { Component, OnInit } from '@angular/core';
import { ApplicationSettingService } from '@app/core/services/application-setting.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  loading: boolean;
  twoFactorEnabled: boolean;

  constructor(
    private applicationSettingService: ApplicationSettingService) { }

  ngOnInit() {
    this.loading = true;
    this.applicationSettingService.getApplicationSettingValue()
      .subscribe(data => {
        this.loading = false;
        this.twoFactorEnabled = data.enableTwoFactorAuth;
      });
  }
}
