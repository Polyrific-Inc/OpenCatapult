import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ProjectService, ProjectDto } from '@app/core';
import { SnackbarService } from '@app/shared';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-clone',
  templateUrl: './project-clone.component.html',
  styleUrls: ['./project-clone.component.css']
})
export class ProjectCloneComponent implements OnInit {
  cloneProjectForm: FormGroup;
  projectInfoForm: FormGroup;
  sourceProject: ProjectDto;
  loading: boolean;
  formSubmitAttempt = false;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private snackBar: SnackbarService,
    private router: Router,
    private route: ActivatedRoute
    ) {
      this.cloneProjectForm = this.fb.group({
        includeJobDefinitions: true,
        includeMembers: true
      });
    }

  ngOnInit() {
  }

  formInitialized(form: FormGroup) {
    this.projectInfoForm = form;
    this.getProject();
  }

  getProject(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.projectService.getProject(id)
      .subscribe(project => {
        this.sourceProject = project;
        this.projectInfoForm.patchValue({
          client: this.sourceProject.client
        });
      });
  }

  onSubmit() {
    this.formSubmitAttempt = true;
    const projectInfo = this.projectInfoForm.value;
    const cloneProjectOption = {
      newProjectName: projectInfo.name,
      displayName: projectInfo.displayName,
      client: projectInfo.client,
      ...this.cloneProjectForm.value
    };

    if (cloneProjectOption.newProjectName === this.sourceProject.name) {
      this.snackBar.open('Please input a different project name');
      return;
    }

    if (this.cloneProjectForm.valid) {
      this.loading = true;
      this.projectService.cloneProject(this.sourceProject.id, cloneProjectOption)
        .subscribe(
            (data: ProjectDto) => {
              this.snackBar.open('The project has been cloned');

              this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                .then(() => this.router.navigate([`project/${data.id}`]));
            },
            err => {
              this.snackBar.open(err);
              this.loading = false;
            });
    }
  }

  onCancelClick() {
    this.router.navigate([`project/${this.sourceProject.id}`]);
  }

}
