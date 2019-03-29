import { Component, OnInit, OnChanges, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { JobTaskDefinitionDto, jobTaskDefinitionTypes, TaskProviderService,
  TaskProviderDto, DeletionJobTaskDefinitionTypes } from '@app/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-job-task-definition-form',
  templateUrl: './job-task-definition-form.component.html',
  styleUrls: ['./job-task-definition-form.component.css']
})
export class JobTaskDefinitionFormComponent implements OnInit, OnChanges {
  @Input() jobTaskDefinition: JobTaskDefinitionDto;
  @Input() disableForm: boolean;
  @Input() isDeletion: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  jobTaskDefinitionForm: FormGroup;
  jobTaskDefinitionTypes: Array<any>;
  taskProviders: TaskProviderDto[];
  allTaskProviders: TaskProviderDto[];

  constructor(
    private fb: FormBuilder,
    private taskProviderService: TaskProviderService) { }

  ngOnInit() {
    this.jobTaskDefinitionTypes = this.isDeletion ? DeletionJobTaskDefinitionTypes : jobTaskDefinitionTypes;

    this.jobTaskDefinitionForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      type: [{value: null, disabled: this.disableForm}],
      provider: [{value: null, disabled: this.disableForm}],
      sequence: this.jobTaskDefinition ? this.jobTaskDefinition.sequence : null
    });

    this.taskProviderService.getTaskProviders('all')
      .subscribe(data => {
        this.allTaskProviders = data;

        if (this.jobTaskDefinition && this.jobTaskDefinition.type) {
          this.onTypeChange(this.jobTaskDefinition.type);
        }

        this.formReady.emit(this.jobTaskDefinitionForm);
      });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.disableForm && !changes.disableForm.firstChange) {
      if (this.disableForm) {
        this.onTypeChange(this.jobTaskDefinition.type);
        this.jobTaskDefinitionForm.patchValue(this.jobTaskDefinition);
        this.jobTaskDefinitionForm.disable();
      } else {
        this.jobTaskDefinitionForm.enable();
      }
    }
  }

  isFieldInvalid(controlName: string, errorCode: string) {
    const control = this.jobTaskDefinitionForm.get(controlName);
    return control.invalid && control.errors && control.getError(errorCode);
  }

  onTypeChange(taskType: string) {
    this.taskProviders = this.allTaskProviders.filter(prov => prov.type === this.taskProviderService.getTaskProviderType(taskType));
  }

  onProviderChanged() {
    this.jobTaskDefinition = {
      configs: {},
      additionalConfigs: {},
      ...this.jobTaskDefinitionForm.value
    };
  }

  onConfigFormReady(form: FormGroup) {
    this.jobTaskDefinitionForm.setControl('configs', form.get('configs'));
    this.jobTaskDefinitionForm.setControl('additionalConfigs', form.get('additionalConfigs'));
  }
}
