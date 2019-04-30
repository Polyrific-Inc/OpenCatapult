import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { ProjectService, ProjectDto, AuthorizePolicy } from '@app/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { SnackbarService } from '@app/shared';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {
  projectForm: FormGroup;
  projectInfoForm: FormGroup;
  project: ProjectDto;
  editing: boolean;
  loading: boolean;
  formSubmitAttempt = false;
  authorizePolicy = AuthorizePolicy;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private snackBar: SnackbarService,
    private route: ActivatedRoute
    ) { }

  ngOnInit() {
    this.projectForm = this.fb.group({
      id: new FormControl({value: null, disabled: true}),
      createdDate: new FormControl({value: null, disabled: true})
    });

    this.getProject();

    this.editing = false;
  }

  getProject(): void {
    this.route.parent.data.subscribe((data: {project: ProjectDto}) => {
      this.project = data.project;
      this.populateForm();
    });
  }

  populateForm() {
    const datePipe = new DatePipe('en-US');
    this.projectForm.patchValue({
      id: this.project.id,
      createdDate: datePipe.transform(this.project.created, 'MMMM d, yyyy')
    });
  }

  /**
   * After a form is initialized, we link it to our main form
   */
  formInitialized(form: FormGroup) {
    this.projectInfoForm = form;
  }

  onSubmit() {
    this.formSubmitAttempt = true;
    if (this.projectInfoForm.valid) {
      this.loading = true;
      this.projectService.updateProject({
        id: this.project.id,
        ...this.projectInfoForm.value
      })
        .subscribe(
            data => {
              this.snackBar.open('Project info has been updated');
              this.loading = false;
              this.project = {
                ...this.projectInfoForm.value,
                id: this.project.id,
                created: this.project.created
              };
              this.editing = false;
            },
            err => {
              this.snackBar.open(err);
              this.loading = false;
            });
    }
  }

  setEditing(editing: boolean) {
    this.editing = editing;
  }
}
