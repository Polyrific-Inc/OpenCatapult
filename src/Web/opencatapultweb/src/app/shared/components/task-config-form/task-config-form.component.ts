import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CreateJobTaskDefinitionDto, TaskProviderService, ExternalServiceService, TaskProviderDto } from '@app/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-task-config-form',
  templateUrl: './task-config-form.component.html',
  styleUrls: ['./task-config-form.component.css']
})
export class TaskConfigFormComponent implements OnInit, OnChanges {
  @Input() task: CreateJobTaskDefinitionDto;
  @Input() showRequiredOnly: boolean;
  taskConfigForm: FormGroup = this.fb.group({
    provider: null,
    externalService: null
  });;
  taskProvider: TaskProviderDto;

  constructor(
    private taskProviderService: TaskProviderService,
    private externalServiceService: ExternalServiceService,
    private fb: FormBuilder
  ) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges){
    if (changes.task) {
      this.taskConfigForm.patchValue({
        provider: this.task.provider
      });

      this.taskProviderService.getTaskProviderByName(this.task.provider)
        .subscribe(data => {
          if (!data) {
            this.taskConfigForm.get('provider').setErrors({
              'notFound': true
            });
            this.taskConfigForm.get('provider').markAsTouched();
          }
          else {
            this.taskProvider = data;

            if (this.taskProvider.requiredServices && this.taskProvider.requiredServices.length > 0){
              for (let requiredService of this.taskProvider.requiredServices) {
                const externalServiceName = `${requiredService}ExternalService`;
                let taskExternalService = this.task.configs ? this.task.configs[externalServiceName] : null;
                this.taskConfigForm.addControl(externalServiceName, new FormControl(taskExternalService, Validators.required));

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

}
