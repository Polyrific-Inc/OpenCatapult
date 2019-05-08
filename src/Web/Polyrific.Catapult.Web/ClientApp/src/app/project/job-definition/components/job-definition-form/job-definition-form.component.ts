import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { JobDefinitionDto } from '@app/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-job-definition-form',
  templateUrl: './job-definition-form.component.html',
  styleUrls: ['./job-definition-form.component.css']
})
export class JobDefinitionFormComponent implements OnInit, OnChanges {
  @Input() jobDefinition: JobDefinitionDto;
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  jobDefinitionForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.jobDefinitionForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required]
    });

    this.formReady.emit(this.jobDefinitionForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.jobDefinition && !changes.jobDefinition.firstChange) {
      this.jobDefinitionForm.patchValue(this.jobDefinition);
    }

    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.jobDefinitionForm.patchValue(this.jobDefinition);
        this.jobDefinitionForm.disable();
      } else {
        this.jobDefinitionForm.enable();
      }
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.jobDefinitionForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
