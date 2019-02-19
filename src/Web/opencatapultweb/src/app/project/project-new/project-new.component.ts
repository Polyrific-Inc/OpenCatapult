import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { NewProjectDto, ProjectService, ProjectTemplateService, ProjectDto } from '@app/core';
import * as jsYaml from 'js-yaml';
import { Router } from '@angular/router';
import { SnackbarService } from '@app/shared';

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
    private snackBar: SnackbarService,
    private router: Router
    ) { }

  ngOnInit() {
    this.projectForm = this.fb.group({
    });

    this.template = this.fb.control("template");

    this.templates = [
      ["sample.yaml", 'sample'],
      ["sample-devops.yaml", 'sample-devops'],
      ["custom", 'Upload template file (.yaml or .yml)']
    ];
  }

  formInitialized(form: FormGroup) {
    this.projectForm = this.fb.group({
      ...this.projectForm.controls,
      ...form.controls
    })
  }

  onSubmit() {
    this.formSubmitAttempt = true;
    if (this.projectForm.valid) {
      this.loading = true;
      this.projectService.createProject(this.projectForm.value)
        .subscribe(
            (data: ProjectDto) => {
              this.snackBar.open("New project has been created");  
              
              this.router.navigate(["project", { dummyData: (new Date).getTime()}])
                .then(() => this.router.navigate([`project/${data.id}`]));
            },
            err => {
              this.snackBar.open(err);
              this.loading = false;
            });
    }
  }

  onTemplateChanged(template) {
    this.projectTemplate = null;

    if (template.value === "custom"){
      this.uploadTemplate = true;
      this.customTemplateFile = null;
    }
    else {      
      if (template.value){
        this.loadTemplate(template.value);
      }

      this.uploadTemplate = false;
    }
  }

  loadTemplate(template) {
    this.projectTemplateService.getTemplate(template)
      .subscribe(templateContent => {
        this.projectTemplate = jsYaml.safeLoad(templateContent);
      },
      err => {        
        this.snackBar.open(err);
      })
  }

  onFileProjectTemplateChanged(event) {
    if (event.target.value){
      this.customTemplateFile = event.target.value.split(/(\\|\/)/g).pop();
  
      let fileReader = new FileReader();
      fileReader.onload = (e) => {
        this.projectTemplate = jsYaml.safeLoad(fileReader.result);
      }
      fileReader.readAsText(event.target.files[0]);
    }
  }

  onJobsFormReady(form: FormArray) {
    this.projectForm.setControl("jobs", form);
  }
}
