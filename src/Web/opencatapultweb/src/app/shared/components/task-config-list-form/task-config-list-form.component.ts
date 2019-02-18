import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CreateJobTaskDefinitionDto } from '@app/core';
import { FormGroup, FormArray, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-task-config-list-form',
  templateUrl: './task-config-list-form.component.html',
  styleUrls: ['./task-config-list-form.component.css']
})
export class TaskConfigListFormComponent implements OnInit {
  @Input() tasks: CreateJobTaskDefinitionDto[];
  @Output() formReady = new EventEmitter<FormArray>();
  tasksForm = this.fb.array([]);

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.formReady.emit(this.tasksForm);
  }

  onConfigFormChanged(form : FormGroup) {
    this.tasksForm.push(form);
  }

}
