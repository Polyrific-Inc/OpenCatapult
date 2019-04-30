import { Component, OnInit } from '@angular/core';
import { ProjectDto, ProjectService, AuthorizePolicy } from '@app/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { ConfirmationWithInputDialogComponent, SnackbarService, ConfirmationDialogComponent } from '@app/shared';

@Component({
  selector: 'app-project-deleting',
  templateUrl: './project-deleting.component.html',
  styleUrls: ['./project-deleting.component.css']
})
export class ProjectDeletingComponent implements OnInit {
  project: ProjectDto;
  authorizePolicy = AuthorizePolicy;
  loading: boolean;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getProject();
  }

  getProject(): void {
    this.route.data.subscribe((data: {project: ProjectDto}) => {
      this.project = data.project;
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

  onRestoreClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Restore Project',
        confirmationText: `Are you sure you want to restore project '${this.project.name}'?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.loading = true;
        this.projectService.restoreProject(this.project.id)
          .subscribe(
              () => {
                this.snackbar.open('The project has been restored');

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
}
