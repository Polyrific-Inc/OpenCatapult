import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CreateJobDefinitionDto } from '@app/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-job-config-form',
  templateUrl: './job-config-form.component.html',
  styleUrls: ['./job-config-form.component.css']
})
export class JobConfigFormComponent implements OnInit, OnChanges {
  @Input() job: CreateJobDefinitionDto;
  @Output() formReady = new EventEmitter<FormGroup>();
  jobForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.jobForm = this.fb.group(
      {
        name: null,
        isDeletion: false,
      }
    );
  }

  ngOnInit() {
    this.formReady.emit(this.jobForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    this.jobForm.patchValue({
      name: this.job.name,
      isDeletion: this.job.isDeletion
    });
  }

  onTaskConfigListChanged(form: FormArray) {
    this.jobForm.setControl('tasks', form);
  }
}
