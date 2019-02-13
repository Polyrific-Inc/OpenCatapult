import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ProjectDto } from '@app/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-project-info-form',
  templateUrl: './project-info-form.component.html',
  styleUrls: ['./project-info-form.component.css']
})
export class ProjectInfoFormComponent implements OnInit, OnChanges {
  @Input() project: ProjectDto;
  @Input() disableForm: boolean;
  @Input() formSubmitAttempt: boolean;
  @Output() formReady = new EventEmitter<FormGroup>();
  projectInfoForm: FormGroup;
  private projectName = new Subject<string>();

  constructor(
    private fb: FormBuilder
    ) { }

  ngOnInit() {
    this.projectInfoForm = this.fb.group({
      name: [{value: null, disabled: this.disableForm}, Validators.required],
      displayName: [{value: null, disabled: this.disableForm}],
      client: {value: null, disabled: this.disableForm}    
    });

    this.normalizeProjectName();

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

  onNameChanged(name: string) {
    this.projectName.next(name);
  }

  normalizeProjectName() {
    this.projectName.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(projectName => {
      projectName = projectName.trim();
      if (projectName.indexOf(' ')){        
        projectName = projectName.replace(/ /g, "-");
      }

      this.projectInfoForm.patchValue({
        name: projectName
      });
    });
  }

  isFieldInvalid(field: string) {
    return (
      (!this.projectInfoForm.get(field).valid && this.projectInfoForm.get(field).touched) ||
      (this.projectInfoForm.get(field).untouched && this.formSubmitAttempt)
    );
  }
  

}
