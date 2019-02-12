import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ProjectDto } from '@app/core';

@Component({
  selector: 'app-project-info-form',
  templateUrl: './project-info-form.component.html',
  styleUrls: ['./project-info-form.component.css']
})
export class ProjectInfoFormComponent implements OnInit, OnChanges {
  @Input() project: ProjectDto;
  @Input() disableForm: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  projectInfoForm: FormGroup;
  private formSubmitAttempt: boolean;

  constructor(
    private fb: FormBuilder
    ) { }

  ngOnInit() {
    this.projectInfoForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      displayName: [{value: null, disabled: this.disableForm}, Validators.required],
      client: {value: null, disabled: this.disableForm}    
    });

    this.formReady.emit(this.projectInfoForm);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.project && !changes.project.firstChange) {
      this.projectInfoForm.patchValue({
        name: this.project.name,
        displayName: this.project.displayName,
        client: this.project.client
      });
    }

    if (changes.disableForm && !changes.disableForm.firstChange){
      if (this.disableForm) {
        this.projectInfoForm.patchValue({
          name: this.project.name,
          displayName: this.project.displayName,
          client: this.project.client
        });
        this.projectInfoForm.disable();
      }
      else {
        this.projectInfoForm.enable();
      }
    }
  }

  isFieldInvalid(field: string) {
    return (
      (!this.projectInfoForm.get(field).valid && this.projectInfoForm.get(field).touched) ||
      (this.projectInfoForm.get(field).untouched && this.formSubmitAttempt)
    );
  }
  

}
