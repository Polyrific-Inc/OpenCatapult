import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-generate-task-config-form',
  templateUrl: './generate-task-config-form.component.html',
  styleUrls: ['./generate-task-config-form.component.css']
})
export class GenerateTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: Map<string, string>;
  @Output() formReady = new EventEmitter<FormGroup>();
  generateConfigForm: FormGroup;
  showForm: boolean;

  constructor(private fb: FormBuilder) {
    this.generateConfigForm = this.fb.group({
      OutputLocation: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Generate){
      this.formReady.emit(this.generateConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.showForm = this.taskType === JobTaskDefinitionType.Generate;

    if (changes.taskConfigs && this.taskConfigs){
      this.generateConfigForm.patchValue(this.taskConfigs);
    }
  }

}
