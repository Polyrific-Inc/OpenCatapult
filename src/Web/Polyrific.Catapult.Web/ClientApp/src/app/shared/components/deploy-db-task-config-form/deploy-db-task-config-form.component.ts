import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { JobTaskDefinitionType } from '@app/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-deploy-db-task-config-form',
  templateUrl: './deploy-db-task-config-form.component.html',
  styleUrls: ['./deploy-db-task-config-form.component.css']
})
export class DeployDbTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Output() formReady = new EventEmitter<FormGroup>();
  deployDbConfigForm: FormGroup;
  showForm: boolean;

  constructor(private fb: FormBuilder) {
    this.deployDbConfigForm = this.fb.group({
      MigrationLocation: null
    });
   }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.DeployDb) {
      this.formReady.emit(this.deployDbConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.showForm = this.taskType === JobTaskDefinitionType.DeployDb;

    if (changes.taskConfigs && this.taskConfigs) {
      this.deployDbConfigForm.patchValue(this.taskConfigs);
    }
  }
}
