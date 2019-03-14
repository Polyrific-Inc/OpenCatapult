import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { CreateJobTaskDefinitionDto, TaskProviderService, ExternalServiceService, TaskProviderDto, ExternalServiceType } from '@app/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-task-config-form',
  templateUrl: './task-config-form.component.html',
  styleUrls: ['./task-config-form.component.css']
})
export class TaskConfigFormComponent implements OnInit, OnChanges {
  @Input() task: CreateJobTaskDefinitionDto;
  @Input() hideTaskInfo: boolean;
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  taskForm: FormGroup;
  taskConfigForm: FormGroup = this.fb.group({});
  taskProvider: TaskProviderDto;
  externalServices$ = this.externalServiceService.externalServices;

  constructor(
    private taskProviderService: TaskProviderService,
    private externalServiceService: ExternalServiceService,
    private fb: FormBuilder
  ) {
    this.taskForm = this.fb.group({
      name: null,
      type: null,
      provider: null,
      sequence: null
    });
    this.taskForm.addControl('configs', this.taskConfigForm);
  }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.task) {
      this.taskForm.patchValue(this.task);

      this.taskProviderService.getTaskProviderByName(this.task.provider)
        .subscribe(taskProvider => {
          if (!taskProvider) {
            this.taskForm.get('provider').setErrors({
              'notFound': true
            });
            this.taskForm.get('provider').markAsTouched();
          } else {
            this.taskProvider = taskProvider;

            if (this.taskProvider.requiredServices && this.taskProvider.requiredServices.length > 0) {
              for (const requiredService of this.taskProvider.requiredServices) {
                const externalServiceName = `${requiredService}ExternalService`;
                const taskExternalService = this.task.configs ? this.task.configs[externalServiceName] : null;
                this.taskConfigForm.setControl(externalServiceName,
                  this.fb.control({ value: taskExternalService, disabled: this.disableForm}, Validators.required));

                if (taskExternalService) {
                  this.externalServiceService.getExternalServiceByName(taskExternalService)
                    .subscribe(extService => {
                      if (!extService) {
                        this.taskConfigForm.get(externalServiceName).setErrors({
                          'notFound': `The external service ${taskExternalService} is not found in the server`
                        });

                        this.taskConfigForm.get(externalServiceName).markAsTouched();
                      }
                    });
                }
              }
            }
          }
        });
    }
  }

  isFieldInvalid(field: string) {
    const control = this.taskConfigForm.get(field);
    return (
      control && !control.valid && control.touched
    );
  }

  getServiceFieldError(field: string) {
    const control = this.taskConfigForm.get(field);

    if (control.errors.required) {
      return `The ${field} is required`;
    } else if (control.errors.notFound) {
      return control.errors.notFound;
    }
  }

  onTaskConfigFormReady(form: FormGroup) {
    Object.keys(form.controls).forEach(key => {
      const control = form.get(key);
      this.taskConfigForm.setControl(key, control);
    });

    if (this.disableForm) {
      this.taskConfigForm.disable();
    } else {
      this.taskConfigForm.enable();
    }
  }

  onAdditionalConfigFormReady(form: FormGroup) {
    this.taskForm.setControl('additionalConfigs', form);
    this.formReady.emit(this.taskForm);
  }

  getExternalServices(serviceType: string) {
    if (serviceType === ExternalServiceType.Azure || serviceType === ExternalServiceType.GitHub) {
      return this.externalServices$.pipe(map(data => {
        return data.filter(s => s.externalServiceTypeName === serviceType);
      }));
    }

    return this.externalServices$;
  }

}
