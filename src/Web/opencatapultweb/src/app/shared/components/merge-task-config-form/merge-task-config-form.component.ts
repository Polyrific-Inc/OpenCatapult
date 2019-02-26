import { Component, OnInit, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-merge-task-config-form',
  templateUrl: './merge-task-config-form.component.html',
  styleUrls: ['./merge-task-config-form.component.css']
})
export class MergeTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Input() taskConfigs: { [key: string]: string };
  @Output() formReady = new EventEmitter<FormGroup>();
  mergeConfigForm: FormGroup;
  showForm: boolean;

  constructor(private fb: FormBuilder) {
    this.mergeConfigForm = this.fb.group({
      Repository: [null, Validators.required]
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Merge) {
      this.formReady.emit(this.mergeConfigForm);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.showForm = this.taskType === JobTaskDefinitionType.Merge;

    if (changes.taskConfigs && this.taskConfigs) {
      this.mergeConfigForm.patchValue(this.taskConfigs);
    }
  }

}
