import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataModelPropertyDto, propertyDataTypes, propertyControlTypes, DataModelDto } from '@app/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { propertyRelationalTypes } from '@app/core/constants/property-relational-types';

@Component({
  selector: 'app-data-model-property-form',
  templateUrl: './data-model-property-form.component.html',
  styleUrls: ['./data-model-property-form.component.css']
})
export class DataModelPropertyFormComponent implements OnInit, OnChanges {
  @Input() dataModelProperty: DataModelPropertyDto;
  @Input() dataModelName: string;
  @Input() relatedDataModels: DataModelDto[];
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  dataModelPropertyForm: FormGroup;
  propertyDataTypes = propertyDataTypes;
  propertyControlTypes = propertyControlTypes;
  propertyRelationalTypes = propertyRelationalTypes;

  private dataModelPropertyName = new Subject<string>();

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.dataModelPropertyForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      label: [{value: null, disabled: this.disableForm}],
      dataType: [{value: null, disabled: this.disableForm}],
      controlType: [{value: null, disabled: this.disableForm}],
      isRequired: [{value: false, disabled: this.disableForm}],
      relatedProjectDataModelId: [{value: null, disabled: this.disableForm}],
      relationalType: [{value: null, disabled: this.disableForm}],
      isManaged: [{value: false, disabled: this.disableForm}]
    });

    this.normalizeDataModelPropertyName();

    this.formReady.emit(this.dataModelPropertyForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.dataModelProperty && !changes.dataModelProperty.firstChange) {
      this.dataModelPropertyForm.patchValue(this.dataModelProperty);
    }

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.dataModelPropertyForm.patchValue(this.dataModelProperty);
        this.dataModelPropertyForm.disable();
      } else {
        this.dataModelPropertyForm.enable();
      }
    }
  }

  onNameChanged(name: string) {
    this.dataModelPropertyName.next(name);
  }

  normalizeDataModelPropertyName() {
    this.dataModelPropertyName.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(propertyName => {
      propertyName = propertyName.trim();
      if (propertyName.indexOf(' ')) {
        propertyName = propertyName.replace(/ /g, '');
      }

      this.dataModelPropertyForm.patchValue({
        name: propertyName
      });
    });
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.dataModelPropertyForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
