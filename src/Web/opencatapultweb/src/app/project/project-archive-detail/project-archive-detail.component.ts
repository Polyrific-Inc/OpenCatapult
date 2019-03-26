import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ProjectDto, ProjectService, AuthorizePolicy } from '@app/core';
import { SnackbarService, ConfirmationDialogComponent, ConfirmationWithInputDialogComponent } from '@app/shared';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-project-archive-detail',
  templateUrl: './project-archive-detail.component.html',
  styleUrls: ['./project-archive-detail.component.css']
})
export class ProjectArchiveDetailComponent implements OnInit {
  projectInfoForm: FormGroup;
  project: ProjectDto;
  loading: boolean;
  formSubmitAttempt = false;
  authorizePolicy = AuthorizePolicy;

  constructor(
    private projectService: ProjectService,
    private snackbar: SnackbarService,
    private router: Router,
    private route: ActivatedRoute,
    private dialog: MatDialog
    ) {
  }

  ngOnInit() {
  }

  formInitialized(form: FormGroup) {
    this.projectInfoForm = form;
    this.getProject();
  }

  getProject(): void {
    this.route.data.subscribe((data: {project: ProjectDto}) => {
      this.project = data.project;
      this.projectInfoForm.patchValue(this.project);
    });
  }

  onActivateClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Activate Project',
        confirmationText: `Are you sure you want to activate project '${this.project.name}'?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.loading = true;
        this.projectService.restoreProject(this.project.id)
          .subscribe(
              () => {
                this.snackbar.open('The project has been activated');

                this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                  .then(() => this.router.navigate([`project/${this.project.id}`]));
              },
              err => {
                this.snackbar.open(err);
                this.loading = false;
              });
      }
    });
  }

  onDeleteClick() {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Project',
        confirmationText: 'Please enter project name to confirm project deletion:',
        confirmationMatch: this.project.name
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.projectService.deleteProject(this.project.id)
          .subscribe(data => {
            this.snackbar.open('Project has been deleted');

            this.router.navigate(['project', { dummyData: (new Date).getTime()}])
              .then(() => this.router.navigate(['project']));
          });
      }
    });
  }

  onExportClick() {
    this.projectService.exportProject(this.project.id)
      .subscribe(data => {
        const element = document.createElement('a');
        element.setAttribute('href', 'data:text/yaml;charset=utf-8,' + encodeURIComponent(data));
        element.setAttribute('download', `${this.project.name}.yaml`);

        element.style.display = 'none';
        document.body.appendChild(element);

        element.click();

        document.body.removeChild(element);
      });
  }

}
