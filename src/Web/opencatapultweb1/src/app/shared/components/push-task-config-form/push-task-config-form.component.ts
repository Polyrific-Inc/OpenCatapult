import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';
import { UtilityService } from '@app/shared/services/utility.service';

@Component({
  selector: 'app-push-task-config-form',
  templateUrl: './push-task-config-form.component.html',
  styleUrls: ['./push-task-config-form.component.css']
})
export class PushTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Output() formReady = new EventEmitter<FormGroup>();
  pushConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder,
    private utilityService: UtilityService
    ) {
    this.pushConfigForm = this.fb.group({
      Repository: [null, Validators.required],
      SourceLocation: null,
      Branch: null,
      CreatePullRequest: null,
      PullRequestTargetBranch: null,
      CommitMessage: null,
      Author: null,
      Email: null
    });
   }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Push) {
      this.formReady.emit(this.pushConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.showForm = this.taskType === JobTaskDefinitionType.Push;

    if (changes.taskConfigs && this.taskConfigs) {
      this.pushConfigForm.patchValue(this.utilityService.convertStringToBoolean(this.taskConfigs));
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.pushConfigForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

}
