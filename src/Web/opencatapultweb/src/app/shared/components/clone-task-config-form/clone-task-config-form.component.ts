import { Component, OnInit, Input, OnChanges, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JobTaskDefinitionType } from '@app/core';

@Component({
  selector: 'app-clone-task-config-form',
  templateUrl: './clone-task-config-form.component.html',
  styleUrls: ['./clone-task-config-form.component.css']
})
export class CloneTaskConfigFormComponent implements OnInit, OnChanges {
  @Input() taskType: string;
  @Output() formReady = new EventEmitter<FormGroup>();
  cloneConfigForm: FormGroup;
  showForm: boolean;

  constructor(
    private fb: FormBuilder
  ) { 
    this.cloneConfigForm = this.fb.group({
      Repository: [null, Validators.required],
      IsPrivateRepository: null,
      CloneLocation:null,
      BaseBranch: null
    });
  }

  ngOnInit() {
    if (this.taskType === JobTaskDefinitionType.Clone){
      this.formReady.emit(this.cloneConfigForm);
    }
  }

  ngOnChanges() {
    this.showForm = this.taskType === JobTaskDefinitionType.Clone;
  }

  
}
