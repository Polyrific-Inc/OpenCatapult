import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { ProjectService, ProjectDto, AuthorizePolicy, ProjectHistoryService, JobDefinitionService } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent, ConfirmationDialogComponent } from '@app/shared';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit, OnDestroy {
  project: ProjectDto;
  activeLink: string;
  authorizePolicy = AuthorizePolicy;
  routerSubscription: Subscription;
  loading: boolean;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private projectHistoryService: ProjectHistoryService,
    private jobDefinitionService: JobDefinitionService,
    private dialog: MatDialog,
    private snackbar: SnackbarService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getProject();
    this.activeLink = this.route.firstChild.snapshot.url.pop().path;
    this.routerSubscription = this.router.events.pipe(filter(e => e instanceof NavigationStart)).subscribe((e: NavigationStart) => {
      if (e.url.includes(`/project/${this.project.id}/`)) {
        this.activeLink = e.url.replace(`/project/${this.project.id}/`, '').split('/')[0];
      }
    });
  }

  ngOnDestroy() {
    this.routerSubscription.unsubscribe();
  }

  getProject(): void {
    this.route.data.subscribe((data: {project: ProjectDto}) => {
      this.activeLink = 'info';

      this.project = data.project;
      this.projectHistoryService.addProjectHistory(this.project);
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
        this.loading = true;
        this.jobDefinitionService.getDeletionJobDefinition(this.project.id)
          .subscribe(deletionJob => {
            if (deletionJob != null) {
              const deleteResourceDialogRef = this.dialog.open(ConfirmationDialogComponent, {
                data: {
                  title: 'Delete project resources',
                  confirmationText: 'Do you want to remove the related resources as well?'
                }
              });

              deleteResourceDialogRef.afterClosed().subscribe(deleteResourceConfirmed => {
                if (deleteResourceConfirmed) {
                  this.projectService.markProjectDeleting(this.project.id)
                    .subscribe(() => {
                      this.snackbar.open('Project is being removed. You will be notified once the process has been done');

                      this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                        .then(() => this.router.navigate(['project']));
                    }, () => this.loading = false);
                } else {
                  this.hardDeleteProject();
                }
              });
            } else {
              this.hardDeleteProject();
            }
          }, () => this.loading = false);
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

  hardDeleteProject() {
    this.projectService.deleteProject(this.project.id)
      .subscribe(data => {
        this.snackbar.open('Project has been deleted');

        this.router.navigate(['project', { dummyData: (new Date).getTime()}])
          .then(() => this.router.navigate(['project']));
      }, () => this.loading = false);
  }
}
