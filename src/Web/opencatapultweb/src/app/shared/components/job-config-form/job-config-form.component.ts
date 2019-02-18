import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CreateJobDefinitionDto, JobTaskDefinitionType } from '@app/core';
import { FormArray, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-job-config-form',
  templateUrl: './job-config-form.component.html',
  styleUrls: ['./job-config-form.component.css']
})
export class JobConfigFormComponent implements OnInit {
  @Input() jobDefinitions: CreateJobDefinitionDto[];
  @Output() onFormChanged = new EventEmitter<FormArray>();
  jobsForm = this.fb.array([]);
  jobTasksForm = this.fb.array([]);

  constructor(private fb: FormBuilder) { 

  }

  ngOnInit() {
    this.jobDefinitions.forEach(job => {
      let jobForm = this.fb.group({
        name: job.name,
        tasks: this.jobTasksForm
      })

      this.jobsForm.push(jobForm);
    });
    this.onFormChanged.emit(this.jobsForm);
  }

  onTaskConfigListChanged(form: FormArray) {
    this.jobTasksForm.push(form);
  }
}
