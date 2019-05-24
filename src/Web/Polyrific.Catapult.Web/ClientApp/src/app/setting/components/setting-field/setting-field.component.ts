import { Component, OnInit, Input } from '@angular/core';
import { ApplicationSettingDto } from '@app/core/models/setting/application-setting-dto';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-setting-field',
  templateUrl: './setting-field.component.html',
  styleUrls: ['./setting-field.component.css']
})
export class SettingFieldComponent implements OnInit {
  @Input() applicationSetting: ApplicationSettingDto;
  @Input() form: FormGroup;

  constructor() { }

  ngOnInit() {
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }
}
