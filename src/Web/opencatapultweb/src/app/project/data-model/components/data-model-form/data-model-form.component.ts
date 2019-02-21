import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { DataModelDto } from '@app/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-data-model-form',
  templateUrl: './data-model-form.component.html',
  styleUrls: ['./data-model-form.component.css']
})
export class DataModelFormComponent implements OnInit, OnChanges {
  @Input() dataModel: DataModelDto;
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  dataModelForm: FormGroup;
  private dataModelName = new Subject<string>();

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.dataModelForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      description: [{value: null, disabled: this.disableForm}],
      label: [{value: null, disabled: this.disableForm}],
      isManaged: [{value: null, disabled: this.disableForm}],
      selectKey: [{value: null, disabled: this.disableForm}]
    });

    this.normalizeDataModelName();

    this.formReady.emit(this.dataModelForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.dataModel && !changes.dataModel.firstChange) {
      this.dataModelForm.patchValue(this.dataModel);
    }

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.dataModelForm.patchValue(this.dataModel);
        this.dataModelForm.disable();
      } else {
        this.dataModelForm.enable();
      }
    }
  }

  onNameChanged(name: string) {
    this.dataModelName.next(name);
  }

  normalizeDataModelName() {
    this.dataModelName.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(dataModelName => {
      dataModelName = dataModelName.trim();
      if (dataModelName.indexOf(' ')) {
        dataModelName = dataModelName.replace(/ /g, '');
      }

      this.dataModelForm.patchValue({
        name: dataModelName
      });
    });
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.dataModelForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
