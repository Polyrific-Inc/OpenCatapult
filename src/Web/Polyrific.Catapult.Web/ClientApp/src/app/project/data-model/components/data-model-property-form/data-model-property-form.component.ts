import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataModelPropertyDto, propertyDataTypes, propertyControlTypes,
  DataModelDto, PropertyDataType, PropertyControlType } from '@app/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { propertyRelationalTypes } from '@app/core/constants/property-relational-types';
import { TextHelperService } from '@app/core/services/text-helper.service';
import { MatSelectChange } from '@angular/material';
import { PropertyRelationalType } from '@app/core/enums/property-relational-type';
import * as pluralize from 'pluralize';

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

  private propertyDefinition = {
    [PropertyDataType.String]: PropertyControlType.InputText,
    [PropertyDataType.Integer]: PropertyControlType.InputNumber,
    [PropertyDataType.Short]: PropertyControlType.InputNumber,
    [PropertyDataType.File]: PropertyControlType.InputFile,
    [PropertyDataType.Double]: PropertyControlType.InputNumber,
    [PropertyDataType.Decimal]: PropertyControlType.InputNumber,
    [PropertyDataType.Float]: PropertyControlType.InputNumber,
    [PropertyDataType.DateTime]: PropertyControlType.Calendar,
    [PropertyDataType.Boolean]: PropertyControlType.Checkbox,
    [PropertyDataType.Guid]: PropertyControlType.InputText,
    [PropertyDataType.DbGeography]: PropertyControlType.InputText
  };

  private propertySuggestion: { [key: string]: { dataType: string, controlType: string }} = {
    ['date']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['lastupdated']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['lastaccessed']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['datecreated']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['duedate']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['publishdate']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['datepublished']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['publishon']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['startdate']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['enddate']: { dataType: PropertyDataType.DateTime, controlType: PropertyControlType.Calendar },
    ['notes']: { dataType: PropertyDataType.String, controlType: PropertyControlType.Textarea},
    ['description']: { dataType: PropertyDataType.String, controlType: PropertyControlType.Textarea },
    ['summary']: { dataType: PropertyDataType.String, controlType: PropertyControlType.Textarea },
    ['name']: { dataType: PropertyDataType.String, controlType: PropertyControlType.InputText },
    ['url']: { dataType: PropertyDataType.String, controlType: PropertyControlType.InputText },
    ['city']: { dataType: PropertyDataType.String, controlType: PropertyControlType.InputText },
    ['title']: { dataType: PropertyDataType.String, controlType: PropertyControlType.InputText },
    ['price']: { dataType: PropertyDataType.Decimal, controlType: PropertyControlType.InputNumber  },
    ['status']: { dataType: PropertyDataType.Boolean, controlType: PropertyControlType.Checkbox }
  };

  private dataModelPropertyName = new Subject<string>();

  constructor(
    private fb: FormBuilder,
    private textHelperService: TextHelperService
    ) { }

  ngOnInit() {
    this.dataModelPropertyForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      label: [{value: null, disabled: this.disableForm}],
      dataType: [{value: PropertyDataType.String, disabled: this.disableForm}],
      controlType: [{value: PropertyControlType.InputText, disabled: this.disableForm}],
      isRequired: [{value: false, disabled: this.disableForm}],
      relatedProjectDataModelId: [{value: null, disabled: this.disableForm}],
      relationalType: [{value: null, disabled: this.disableForm}],
      isManaged: [{value: false, disabled: this.disableForm}]
    });

    this.normalizeDataModelPropertyName();

    this.formReady.emit(this.dataModelPropertyForm);
  }

  ngOnChanges(changes: SimpleChanges) {
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

  onDataTypeChanged(event: MatSelectChange) {
    this.dataModelPropertyForm.patchValue({controlType: this.propertyDefinition[event.value]});
  }

  onRelatedDataModelChanged(event: MatSelectChange) {
    // @ts-ignore
    this.dataModelPropertyName.next(event.source.selected.viewValue);

    this.dataModelPropertyForm.patchValue({
      dataType: PropertyDataType.String,
      controlType: PropertyControlType.InputText
    });
  }

  onRelationalTypeChanged(event: MatSelectChange) {
    if (!this.dataModelPropertyForm.value.name) {
      return;
    }

    if (event.value === PropertyRelationalType.OneToOne) {
      this.dataModelPropertyName.next(pluralize.singular(this.dataModelPropertyForm.value.name));
    } else {
      this.dataModelPropertyName.next(pluralize.plural(this.dataModelPropertyForm.value.name));
    }
  }

  normalizeDataModelPropertyName() {
    this.dataModelPropertyName.pipe(
      debounceTime(200),
      distinctUntilChanged()
    ).subscribe(propertyName => {
      propertyName = propertyName.trim();
      if (propertyName.indexOf(' ')) {
        propertyName = propertyName.replace(/ /g, '');
      }

      this.dataModelPropertyForm.patchValue({
        name: propertyName,
        label: this.textHelperService.humanizeText(propertyName)
      });

      const suggestion = this.propertySuggestion[propertyName.toLowerCase()];
      if (suggestion) {
        this.dataModelPropertyForm.patchValue({
          dataType: suggestion.dataType,
          controlType: suggestion.controlType
        });
      }
    });
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.dataModelPropertyForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
