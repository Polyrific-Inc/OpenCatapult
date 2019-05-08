import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ExternalServicePropertyDto } from '@app/core/models/external-service/external-service-property-dto';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-external-service-property-form',
  templateUrl: './external-service-property-form.component.html',
  styleUrls: ['./external-service-property-form.component.css']
})
export class ExternalServicePropertyFormComponent implements OnInit, OnDestroy {
  @Input() externalServiceProperty: ExternalServicePropertyDto;
  @Input() form: FormGroup;
  @Input() isRequired: boolean;
  @Input() disabledForm: boolean;
  formControl: FormControl;
  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    const validators = [];
    const isPropertyRequired = this.isRequired || this.externalServiceProperty.isRequired;

    if (isPropertyRequired) {
      validators.push(Validators.required);
    }

    this.formControl = this.fb.control({value: null, disabled: this.disabledForm},
      Validators.compose(validators));
    this.form.setControl(this.externalServiceProperty.name, this.formControl);
    setTimeout(() => { this.form.updateValueAndValidity(); });
  }

  ngOnDestroy() {
    this.form.removeControl(this.externalServiceProperty.name);
    setTimeout(() => { this.form.updateValueAndValidity(); });
  }

  isFieldInvalid(errorCode: string) {
    return this.formControl.invalid && this.formControl.errors && this.formControl.getError(errorCode);
  }

}
