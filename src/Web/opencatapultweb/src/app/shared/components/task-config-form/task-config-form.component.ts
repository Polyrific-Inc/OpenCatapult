import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { CreateJobTaskDefinitionDto, TaskProviderService, ExternalServiceService, TaskProviderDto, JobTaskDefinitionType } from '@app/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-task-config-form',
  templateUrl: './task-config-form.component.html',
  styleUrls: ['./task-config-form.component.css']
})
export class TaskConfigFormComponent implements OnInit, OnChanges {
  @Input() task: CreateJobTaskDefinitionDto;
  @Output() formReady = new EventEmitter<FormGroup>();
  taskForm: FormGroup;
  taskConfigForm: FormGroup = this.fb.group({});;
  taskProvider: TaskProviderDto;

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
    });;
    this.taskForm.addControl("configs", this.taskConfigForm);
  }

  ngOnInit() {
    this.formReady.emit(this.taskForm);
  }

  ngOnChanges(changes: SimpleChanges){
    if (changes.task) {
      this.taskForm.patchValue(this.task);

      this.taskProviderService.getTaskProviderByName(this.task.provider)
        .subscribe(data => {
          if (!data) {
            this.taskForm.get('provider').setErrors({
              'notFound': true
            });
            this.taskForm.get('provider').markAsTouched();
          }
          else {
            this.taskProvider = data;

            if (this.taskProvider.requiredServices && this.taskProvider.requiredServices.length > 0){
              for (let requiredService of this.taskProvider.requiredServices) {
                const externalServiceName = `${requiredService}ExternalService`;
                let taskExternalService = this.task.configs ? this.task.configs[externalServiceName] : null;
                this.taskConfigForm.setControl(externalServiceName, new FormControl(taskExternalService, Validators.required));

                if (taskExternalService){
                  this.externalServiceService.getExternalServiceByName(taskExternalService)
                    .subscribe(data => {
                      if (!data){
                        this.taskConfigForm.get(externalServiceName).setErrors({
                          'notFound': `The external service ${taskExternalService} is not found in the server`
                        });
  
                        this.taskConfigForm.get(externalServiceName).markAsTouched();
                      }
                    })
                }
              }
            }            
          }
        });
    }
  }

  isFieldInvalid(field: string) {
    let control = this.taskConfigForm.get(field);
    return (
      control && !control.valid && control.touched
    );
  }

  getServiceFieldError(field: string) {    
    let control = this.taskConfigForm.get(field);

    if (control.errors.required) {
      return `The ${field} is required`
    }
    else if (control.errors.notFound) {
      return control.errors.notFound;
    }
  }

  onTaskConfigFormReady(form: FormGroup) {    
    Object.keys(form.controls).forEach(key => {
      let control = form.get(key);
      this.taskConfigForm.setControl(key, control);
    });
  }

  onAdditionalConfigFormReady(form: FormGroup) {
    this.taskForm.setControl("additionalConfigs", form);
  }

}
