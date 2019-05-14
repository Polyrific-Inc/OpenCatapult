import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-delete-repository-config-form',
  templateUrl: './delete-repository-config-form.component.html',
  styleUrls: ['./delete-repository-config-form.component.css']
})
export class DeleteRepositoryConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Input() skipValidation: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  deleteRepositoryConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder
  ) {
    this.deleteRepositoryConfigForm = this.fb.group({
      Repository: null,
      IsPrivateRepository: null,
      CloneLocation: null,
      BaseBranch: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.DeleteRepository) {
      if (!this.skipValidation) {
        this.deleteRepositoryConfigForm.get('Repository').setValidators(Validators.required);
      }

      this.formReady.emit(this.deleteRepositoryConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.showForm = this.taskType === JobTaskDefinitionType.DeleteRepository;

    if (changes.taskConfigs && this.taskConfigs) {
      this.deleteRepositoryConfigForm.patchValue(this.taskConfigs);
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.deleteRepositoryConfigForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
