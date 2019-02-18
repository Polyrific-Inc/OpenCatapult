import { Component, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-test-task-config-form',
  templateUrl: './test-task-config-form.component.html',
  styleUrls: ['./test-task-config-form.component.css']
})
export class TestTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Output() formReady = new EventEmitter<FormGroup>();
  testConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder
  ) {     
    this.testConfigForm = this.fb.group({
      TestLocation: null,
      ContinueWhenError: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Test){      
      this.formReady.emit(this.testConfigForm);
    }
  }

  ngOnChanges() {
    this.showForm = this.taskType === JobTaskDefinitionType.Test;
  }

}
