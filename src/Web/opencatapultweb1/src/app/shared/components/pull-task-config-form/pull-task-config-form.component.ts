import { Component, OnInit, Input, OnChanges, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';
import { UtilityService } from '@app/shared/services/utility.service';

@Component({
  selector: 'app-pull-task-config-form',
  templateUrl: './pull-task-config-form.component.html',
  styleUrls: ['./pull-task-config-form.component.css']
})
export class PullTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Output() formReady = new EventEmitter<FormGroup>();
  pullConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder,
    private utilityService: UtilityService
  ) {
    this.pullConfigForm = this.fb.group({
      Repository: [null, Validators.required],
      IsPrivateRepository: false,
      RepositoryLocation: null,
      BaseBranch: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Pull) {
      this.formReady.emit(this.pullConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    this.showForm = this.taskType === JobTaskDefinitionType.Pull;

    if (changes.taskConfigs && this.taskConfigs) {
      this.pullConfigForm.patchValue(this.utilityService.convertStringToBoolean(this.taskConfigs));
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.pullConfigForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }


}
