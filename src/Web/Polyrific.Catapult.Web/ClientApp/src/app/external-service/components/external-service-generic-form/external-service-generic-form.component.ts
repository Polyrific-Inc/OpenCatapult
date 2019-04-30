import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'app-external-service-generic-form',
  templateUrl: './external-service-generic-form.component.html',
  styleUrls: ['./external-service-generic-form.component.css']
})
export class ExternalServiceGenericFormComponent implements OnInit {
  @Input() form: FormGroup;
  @Input() disableForm: boolean;
  @Input() index: number;
  @Output() deleteClicked = new EventEmitter<number>();

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    if (!this.form.get('key')) {
      this.form.setControl('key', this.fb.control({value: null, disabled: this.disableForm}, Validators.required));
    }

    if (!this.form.get('value')) {
      this.form.setControl('value', this.fb.control({value: null, disabled: this.disableForm}, Validators.required));
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.form.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onDeleteClick() {
    this.deleteClicked.emit(this.index);
  }

}
