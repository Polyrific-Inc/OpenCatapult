import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { NewProjectDto, ProjectService, ProjectTemplateService, ProjectDto, YamlService, ExternalServiceService } from '@app/core';
import { Router } from '@angular/router';
import { SnackbarService } from '@app/shared';
import { AuthService } from '@app/core/auth/auth.service';

@Component({
  selector: 'app-project-new',
  templateUrl: './project-new.component.html',
  styleUrls: ['./project-new.component.css']
})
export class ProjectNewComponent implements OnInit {
  projectForm: FormGroup;
  template: FormControl;
  project: NewProjectDto;
  loading: boolean;
  formSubmitAttempt = false;
  templates: [string, string][];
  uploadTemplate: boolean;
  projectTemplate: NewProjectDto;
  customTemplateFile: string;

  constructor(
    private fb: FormBuilder,
    private projectService: ProjectService,
    private projectTemplateService: ProjectTemplateService,
    private externalServiceService: ExternalServiceService,
    private authService: AuthService,
    private yamlService: YamlService,
    private snackBar: SnackbarService,
    private router: Router
    ) { }

  ngOnInit() {
    this.projectForm = this.fb.group({
    });

    this.externalServiceService.getExternalServices().subscribe();

    this.template = this.fb.control('template');

    this.templates = [
      ['sample.yaml', 'sample'],
      ['sample-devops.yaml', 'sample-devops'],
      ['custom', 'Upload template file (.yaml or .yml)']
    ];
  }

  formInitialized(form: FormGroup) {
    this.projectForm = form;
  }

  onSubmit() {
    this.formSubmitAttempt = true;
    if (this.projectForm.valid) {
      this.loading = true;
      const newProject = {
        ...this.projectForm.value
      };

      if (this.projectTemplate != null && this.projectTemplate.models) {
        newProject.models = this.projectTemplate.models;
      }

      if (this.projectTemplate != null && this.projectTemplate.members) {
        newProject.members = this.projectTemplate.members;
      }

      if (this.projectTemplate != null && this.projectTemplate.jobs) {
        newProject.jobs = this.projectTemplate.jobs;
      }

      this.projectService.createProject(newProject)
        .subscribe(
            (data: ProjectDto) => {
              this.snackBar.open('New project has been created');
              this.authService.refreshSession().subscribe(() => {
                this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                  .then(() => this.router.navigate([`project/${data.id}`]));
              });
            },
            err => {
              this.snackBar.open(err);
              this.loading = false;
            });
    } else {
      this.validateAllFormFields(this.projectForm);
    }
  }

  validateAllFormFields(formGroup: any) {
    Object.keys(formGroup.controls).forEach(field => {
        const control = formGroup.get(field);

        if (control instanceof FormControl) {
            control.markAsTouched({ onlySelf: true });
        } else if (control instanceof FormGroup) {
            this.validateAllFormFields(control);
        } else if (control instanceof FormArray) {
            this.validateAllFormFields(control);
        }
    });
  }

  onTemplateChanged(template) {
    this.projectTemplate = null;

    if (template.value === 'custom') {
      this.uploadTemplate = true;
      this.customTemplateFile = null;
    } else {
      if (template.value) {
        this.loadTemplate(template.value);
      }

      this.uploadTemplate = false;
    }
  }

  loadTemplate(template) {
    this.projectTemplateService.getTemplate(template)
      .subscribe(templateContent => {
        this.projectTemplate = this.yamlService.deserialize(templateContent);
      },
      err => {
        this.snackBar.open(err);
      });
  }

  onFileProjectTemplateChanged(event) {
    if (event.target.value) {
      this.customTemplateFile = event.target.value.split(/(\\|\/)/g).pop();

      const fileReader = new FileReader();
      fileReader.onload = (e) => {
        this.projectTemplate = this.yamlService.deserialize(fileReader.result);
      };
      fileReader.readAsText(event.target.files[0]);
    }
  }
}
