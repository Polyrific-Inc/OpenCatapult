import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService, ProjectDto, AuthorizePolicy } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent, ConfirmationDialogComponent } from '@app/shared';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: ProjectDto;
  activeLink: string;
  authorizePolicy = AuthorizePolicy;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getProject();
    this.activeLink = this.route.firstChild.snapshot.url.pop().path;
  }

  getProject(): void {
    this.route.params.subscribe(params => {
      this.activeLink = 'info';

      const id = +params.projectId;
      this.projectService.getProject(id)
        .subscribe(project => this.project = project);
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

  onArchiveClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Confirm Archive Project',
        confirmationText: `Are you sure you want to archive project '${this.project.name}'?`
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.projectService.archiveProject(this.project.id)
          .subscribe(data => {
            this.snackbar.open('Project has been archived');
            this.router.navigate(['project', { dummyData: (new Date).getTime()}])
              .then(() => this.router.navigate([`project/archive/${this.project.id}`]));
          });
      }
    });
  }

}
