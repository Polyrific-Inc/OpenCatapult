import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService, ProjectDto } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent } from '@app/shared';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: ProjectDto;
  activeLink: string;

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

  getProject() : void {
    this.route.params.subscribe(params => {      
      this.activeLink = 'info';

      const id = +params.id;
      this.projectService.getProject(id)
        .subscribe(project => this.project = project);
    })
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
      if (confirmed){
        this.projectService.deleteProject(this.project.id)
          .subscribe(data => {
            this.snackbar.open('Project has been deleted');
            this.router.navigate(['project']);
          })
      }
    })
  }

}
