import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { JobTaskDefinitionType } from '@app/core';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-deploy-task-config-form',
  templateUrl: './deploy-task-config-form.component.html',
  styleUrls: ['./deploy-task-config-form.component.css']
})
export class DeployTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: Map<string, string>;
  @Output() formReady = new EventEmitter<FormGroup>();
  deployConfigForm: FormGroup;
  showForm: boolean;

  constructor(private fb: FormBuilder) {    
    this.deployConfigForm = this.fb.group({
      ArtifactLocation: null
    });
   }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Deploy){      
      this.formReady.emit(this.deployConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.showForm = this.taskType === JobTaskDefinitionType.Deploy;

    if (changes.taskConfigs && this.taskConfigs){
      this.deployConfigForm.patchValue(this.taskConfigs);
    }
  }

}
