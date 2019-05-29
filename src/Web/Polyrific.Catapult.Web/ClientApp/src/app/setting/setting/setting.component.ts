import { Component, OnInit } from '@angular/core';
import { ApplicationSettingService } from '@app/core/services/application-setting.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ApplicationSettingDto } from '@app/core/models/setting/application-setting-dto';
import { SnackbarService } from '@app/shared';
import { UtilityService } from '@app/shared/services/utility.service';

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.css']
})
export class SettingComponent implements OnInit {
  form: FormGroup;
  loading: boolean;
  editing: boolean;
  applicationSettingValue: { [key: string]: string};
  applicationSettings: ApplicationSettingDto[];

  constructor(
    private applicationSettingService: ApplicationSettingService,
    private fb: FormBuilder,
    private snackbar: SnackbarService,
    private utilityService: UtilityService) { }

  ngOnInit() {
    this.loading = true;
    this.applicationSettingService.getApplicationSettings()
      .subscribe(data => {
        // @ts-ignore
        this.applicationSettingValue = this.utilityService.convertStringToBoolean(data.reduce((agg, curr) => {
          agg[curr.key] = curr.value;
          return agg;
        }, {}));

        this.form = this.fb.group(this.applicationSettingValue);
        this.form.disable();
        this.applicationSettings = data;
        this.loading = false;
      });
  }

  onSubmit() {
    if (this.form.valid) {
      this.loading = true;
      this.applicationSettingService.updateApplicationSetting({
        updatedSettings: this.form.value
      })
        .subscribe(
            () => {
              this.loading = false;
              this.editing = false;
              this.form.disable();
              this.snackbar.open('Application setting has been updated');
            },
            err => {
              this.snackbar.open(err);
              this.loading = false;
            });
    }
  }

  onEditClick() {
    this.form.enable();
    this.editing = true;
  }

  onCancelClick() {
    this.form.disable();
    this.form.patchValue(this.applicationSettingValue);
    this.editing = false;
  }

}
