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
  @Output() formChanged = new EventEmitter<FormArray>();
  taskConfigsForm = this.fb.array([]);
  tasksForm = this.fb.array([]);

  constructor(private fb: FormBuilder) { }

  ngOnInit() {
    this.tasks.forEach(task => {
      let taskForm = this.fb.group({
        name: task.name,
        configs: this.taskConfigsForm
      })

      this.tasksForm.push(taskForm);
    });
    this.formChanged.emit(this.tasksForm);
  }

  onConfigFormChanged(form : FormGroup) {
    this.taskConfigsForm.push(form);
  }

}
