import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CreateJobDefinitionDto } from '@app/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-job-list-form',
  templateUrl: './job-list-form.component.html',
  styleUrls: ['./job-list-form.component.css']
})
export class JobListFormComponent implements OnInit {

  @Input() jobDefinitions: CreateJobDefinitionDto[];
  @Output() formReady = new EventEmitter<FormArray>();
  jobsForm = this.fb.array([]);

  constructor(private fb: FormBuilder) { 

  }

  ngOnInit() {
    this.formReady.emit(this.jobsForm);
  }

  onJobFormReady(form: FormGroup) {
    this.jobsForm.push(form);
  }

}
