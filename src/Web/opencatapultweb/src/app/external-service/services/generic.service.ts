import { Injectable } from '@angular/core';
import { ExternalServiceDto, genericExternalService } from '@app/core';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';

@Injectable()
export class GenericService {

  constructor(private fb: FormBuilder) { }

  patchFormValue(form: FormGroup, genericForm: FormArray, dto: ExternalServiceDto) {
    form.patchValue(dto);

    if (dto.externalServiceTypeId === genericExternalService) {
      while (genericForm.length !== 0) {
        genericForm.removeAt(0);
      }

      const configArray = [];
      for (const key in dto.config) {
        if (dto.config.hasOwnProperty(key)) {
          genericForm.push(this.fb.group({
            key: [{value: key, disabled: true}, Validators.required],
            value: [{value: dto.config[key], disabled: true}, Validators.required]
          }));
          configArray.push({key: key, value: dto.config[key]});

        }
      }

      genericForm.patchValue(configArray);
    }
  }

  getFormValue(form: FormGroup, genericForm: FormArray) {
    const formValue: ExternalServiceDto = form.value;

    if (formValue.externalServiceTypeId === genericExternalService && genericForm && genericForm.controls.length > 0) {
      const arrayValue = genericForm.value;
      const config: {[key: string]: string} = {};

      for (const genericItem of arrayValue) {
        config[genericItem.key] = genericItem.value;
      }

      return {
        ...formValue,
        config: config
      };
    }

    return formValue;
  }
}
