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
  @Input() skipValidation: boolean;
  additionalConfigFile: string;

  constructor() { }

  ngOnInit() {
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onFileChanged(event) {
    if (event.target.value) {
      this.additionalConfigFile = event.target.value.split(/(\\|\/)/g).pop();

      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        this.form.get(this.additionalConfig.name).setValue(fileReader.result);
      };
      fileReader.readAsText(event.target.files[0]);
    }
  }

}
