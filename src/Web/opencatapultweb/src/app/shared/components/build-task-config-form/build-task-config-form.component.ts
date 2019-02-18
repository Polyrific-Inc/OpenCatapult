import { Component, OnInit, OnChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-build-task-config-form',
  templateUrl: './build-task-config-form.component.html',
  styleUrls: ['./build-task-config-form.component.css']
})
export class BuildTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Output() formReady = new EventEmitter<FormGroup>();
  buildConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder
  ) { 
    this.buildConfigForm = this.fb.group({
      SourceLocation: null,
      OutputArtifactLocation: null
    });
  }

  ngOnInit() {
  }

  ngOnChanges() {
    this.showForm = this.taskType === JobTaskDefinitionType.Build;
  }



}
