import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ProjectDto, ProjectService, AuthorizePolicy } from '@app/core';
import { SnackbarService, ConfirmationDialogComponent } from '@app/shared';
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
    this.route.params.subscribe(params => {
      const id = +params.projectId;
      this.projectService.getProject(id)
        .subscribe(project => {
          this.project = project;
          this.projectInfoForm.patchValue(this.project);
        });
    });
  }

  onActivateClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Archive Project',
        confirmationText: `Are you sure you want to archive project '${this.project.name}'?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.loading = true;
        this.projectService.archiveProject(this.project.id)
          .subscribe(data => {
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
          });
      }
    });
  }

}
