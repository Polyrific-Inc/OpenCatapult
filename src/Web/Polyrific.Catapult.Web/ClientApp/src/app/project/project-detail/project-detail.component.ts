import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, NavigationStart } from '@angular/router';
import { ProjectService, ProjectDto, AuthorizePolicy, ProjectHistoryService, JobDefinitionService, JobQueueService } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent, ConfirmationDialogComponent } from '@app/shared';
import { filter } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { MessageDialogComponent } from '@app/shared/components/message-dialog/message-dialog.component';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: ProjectDto;
  authorizePolicy = AuthorizePolicy;
  routerSubscription: Subscription;
  loading: boolean;

  constructor(
    private route: ActivatedRoute,
    private projectService: ProjectService,
    private projectHistoryService: ProjectHistoryService,
    private jobDefinitionService: JobDefinitionService,
    private jobQueueService: JobQueueService,
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
      this.projectHistoryService.addProjectHistory(this.project);
    });
  }

  onDeleteClick() {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Project',
        confirmationText: `Please enter project name (${this.project.name}) to confirm project deletion:`,
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

                      this.projectHistoryService.deleteProjectHistory(this.project.id);

                      this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                        .then(() => this.router.navigate(['project']));
                    }, (err) => {
                      this.snackbar.open(err);
                      this.router.navigate(['project', { dummyData: (new Date).getTime()}])
                        .then(() => this.router.navigate(['project']));
                      this.loading = false;
                    });
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

        this.projectHistoryService.deleteProjectHistory(this.project.id);

        this.router.navigate(['project', { dummyData: (new Date).getTime()}])
          .then(() => this.router.navigate(['project']));
      }, () => this.loading = false);
  }

  onQueueJobClick() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Queue Default Job Definition',
        confirmationText: `Are you sure you want to add default job to the queue?`
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.jobQueueService.addDefaultJobQueue(this.project.id, {
          projectId: this.project.id,
          jobType: null,
          originUrl: window.location.origin,
          jobDefinitionId: null
        }).subscribe(data => this.router.navigateByUrl(`project/${this.project.id}/job-queue`),
          err => {
            if (err.includes('have invalid task(s): External service') && err.includes('is not found')) {
              const addServiceDialogRef = this.dialog.open(ConfirmationDialogComponent, {
                data: {
                  title: 'Add External Service?',
                  confirmationText: `${err}. Do you want to create it now?`
                }
              });

              addServiceDialogRef.afterClosed().subscribe(confirmed => {
                if (confirmed) {
                  this.router.navigateByUrl('/service?newService=true');
                }
              });
            } else if (err.includes('does not have a default job definition')) {
              // Stay on the page
              this.dialog.open(MessageDialogComponent, {
                data: {
                  title: 'Queue Job Failed',
                  // tslint:disable-next-line: max-line-length
                  message: `Project ${this.project.name} does not have a default job definition`
                }
              });
            } else {
              // Redirect to the job definition page
              const failedDialogRef = this.dialog.open(MessageDialogComponent, {
                data: {
                  title: 'Queue Job Failed',
                  // tslint:disable-next-line: max-line-length
                  message: `The default job in project ${this.project.name} has incomplete task configs. Please complete them before putting this job in queue.`
                }
              });

              failedDialogRef.afterClosed().subscribe(() => {
                this.router.navigate(['job-definition'], { queryParams: { showDefaultJob: true }, relativeTo: this.route });
              });
            }
          });
      }
    });
  }
}
