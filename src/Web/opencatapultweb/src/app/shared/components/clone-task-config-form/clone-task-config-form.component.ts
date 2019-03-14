import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-clone-task-config-form',
  templateUrl: './clone-task-config-form.component.html',
  styleUrls: ['./clone-task-config-form.component.css']
})
export class CloneTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Output() formReady = new EventEmitter<FormGroup>();
  cloneConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder
  ) {
    this.cloneConfigForm = this.fb.group({
      Repository: [null, Validators.required],
      IsPrivateRepository: null,
      CloneLocation: null,
      BaseBranch: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Clone) {
      this.formReady.emit(this.cloneConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.showForm = this.taskType === JobTaskDefinitionType.Clone;

    if (changes.taskConfigs && this.taskConfigs) {
      this.cloneConfigForm.patchValue(this.taskConfigs);
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.cloneConfigForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }


}
