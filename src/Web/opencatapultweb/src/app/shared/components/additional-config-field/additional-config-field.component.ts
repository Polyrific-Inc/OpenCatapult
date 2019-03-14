import { Component, OnInit, Input } from '@angular/core';
import { AdditionalConfigDto } from '@app/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-additional-config-field',
  templateUrl: './additional-config-field.component.html',
  styleUrls: ['./additional-config-field.component.css']
})
export class AdditionalConfigFieldComponent implements OnInit {
  @Input() additionalConfig: AdditionalConfigDto;
  @Input() form: FormGroup;
  constructor() { }

  ngOnInit() {
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
