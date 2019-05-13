import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { Validators, FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { ExternalServiceDto, genericExternalService, ExternalServiceType } from '@app/core';
import { ExternalServiceTypeDto } from '@app/core/models/external-service/external-service-type-dto';
import { ExternalServiceTypeService } from '@app/core/services/external-service-type.service';
import { Observable } from 'rxjs';
import { ExternalServicePropertyDto } from '@app/core/models/external-service/external-service-property-dto';
import { tap } from 'rxjs/operators';
import { MatSelectChange, MatOption } from '@angular/material';

export interface ExternalServiceFormOutput {
  mainForm: FormGroup;
  genericConfigForm: FormArray;
}

@Component({
  selector: 'app-external-service-form',
  templateUrl: './external-service-form.component.html',
  styleUrls: ['./external-service-form.component.css']
})
export class ExternalServiceFormComponent implements OnInit, OnChanges {
  @Input() externalService: ExternalServiceDto;
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<ExternalServiceFormOutput>();
  externalServiceForm: FormGroup;
  externalServiceConfigForm: FormGroup;
  externalServiceTypes$: Observable<ExternalServiceTypeDto[]>;
  externalServiceProperties: ExternalServicePropertyDto[];
  genericServiceProperties: FormArray;
  showGenericProperties: boolean;

  constructor(
    private fb: FormBuilder,
    private serviceTypeService: ExternalServiceTypeService
    ) { }

  ngOnInit() {
    this.externalServiceConfigForm = this.fb.group({});
    this.externalServiceForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      description: [{value: null, disabled: this.disableForm}],
      externalServiceTypeId: [{value: null, disabled: this.disableForm}, Validators.required],
      config: this.externalServiceConfigForm,
      isGlobal: [{value: false, disabled: this.disableForm}]
    });

    this.genericServiceProperties = this.fb.array([]);

    this.externalServiceTypes$ = this.serviceTypeService.getExternalServiceTypes(false);

    if (this.externalService && this.externalService.externalServiceTypeId) {
      this.getExternalServiceType(this.externalService.externalServiceTypeId)
        .subscribe(data => {
          this.formReady.emit({
            mainForm: this.externalServiceForm,
            genericConfigForm: this.genericServiceProperties
          });
        });
    } else {
      this.formReady.emit({
        mainForm: this.externalServiceForm,
        genericConfigForm: this.genericServiceProperties
      });
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.externalService && !changes.externalService.firstChange) {
      this.externalServiceForm.patchValue(this.externalService);
    }

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.externalServiceForm.patchValue(this.externalService);
        this.externalServiceForm.disable();
      } else {
        this.externalServiceForm.enable();
      }
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.externalServiceForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  isPropertyHidden(serviceProperty: ExternalServicePropertyDto) {
    if (serviceProperty.additionalLogic && serviceProperty.additionalLogic.hideCondition) {
      const currentControl = this.externalServiceConfigForm.get(serviceProperty.additionalLogic.hideCondition.propertyName);

      if (currentControl) {
        return currentControl.value === serviceProperty.additionalLogic.hideCondition.propertyValue;
      }
    }

    return false;
  }

  isPropertyRequired(serviceProperty: ExternalServicePropertyDto) {
    if (serviceProperty.additionalLogic && serviceProperty.additionalLogic.requiredCondition) {
      const currentControl = this.externalServiceConfigForm.get(serviceProperty.additionalLogic.requiredCondition.propertyName);

      if (currentControl) {
        return currentControl.value === serviceProperty.additionalLogic.requiredCondition.propertyValue;
      }
    }

    return false;
  }

  getExternalServiceType(serviceTypeId: number) {
    return this.serviceTypeService.getExternalServiceType(serviceTypeId)
      .pipe(tap(data => {
        if (data.id === genericExternalService) {
          this.showGenericProperties = true;
          this.externalServiceProperties = [];
        } else {
          this.showGenericProperties = false;
          this.externalServiceProperties = data.externalServiceProperties;
        }
      }));
  }

  onServiceTypeChanged(control: MatSelectChange) {
    let defaultName = '';
    if ((<MatOption>control.source.selected).viewValue === ExternalServiceType.Azure) {
      defaultName = 'azure-default';
    } else if ((<MatOption>control.source.selected).viewValue === ExternalServiceType.GitHub) {
      defaultName = 'github-default';
    }

    this.getExternalServiceType(control.value).subscribe();
    this.externalServiceForm.patchValue({
      name: defaultName
    });
  }

  onAddGenericPropertyClick() {
    this.genericServiceProperties.push(new FormGroup({}));
  }

  onDeleteClicked(idx: number) {
    this.genericServiceProperties.removeAt(idx);
  }
}
