import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ProjectService, ProjectDto } from '@app/core';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {
  projectInfoForm: FormGroup;
  project: ProjectDto;
  editing: boolean;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService
    ) { }

  ngOnInit() {
    this.projectInfoForm = this.fb.group({
      id: new FormControl({value: null, disabled: true}),
      created: new FormControl({value: null, disabled: true})
    });

    this.projectService.getCurrentProject().subscribe(data => 
      {
        this.project = data;
        this.populateForm();
      });
    
      this.editing = false;
  }

  populateForm() {
    this.projectInfoForm.patchValue(this.project);
  }

  /**
   * After a form is initialized, we link it to our main form
   */
  formInitialized(form: FormGroup) {
    this.projectInfoForm = this.fb.group({
      ...this.projectInfoForm.controls,
      ...form.controls
    })
  }

  onSubmit() {

  }

  setEditing(editing : boolean)
  {
    this.editing = editing;
  }
}
