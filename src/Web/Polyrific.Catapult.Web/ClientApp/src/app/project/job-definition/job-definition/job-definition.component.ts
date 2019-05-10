import { Component, OnInit } from '@angular/core';
import { JobDefinitionDto, JobDefinitionService, JobQueueService, ProjectService, AuthorizePolicy } from '@app/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatCheckboxChange } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent, ConfirmationDialogComponent } from '@app/shared';
import { JobDefinitionNewDialogComponent } from '../components/job-definition-new-dialog/job-definition-new-dialog.component';
import { JobDefinitionInfoDialogComponent } from '../components/job-definition-info-dialog/job-definition-info-dialog.component';
import { JobTaskDefinitionNewDialogComponent } from '../components/job-task-definition-new-dialog/job-task-definition-new-dialog.component';
import { MessageDialogComponent } from '@app/shared/components/message-dialog/message-dialog.component';

interface JobDefinitionViewModel extends JobDefinitionDto {
  selected: boolean;
  expanded: boolean;
}

@Component({
  selector: 'app-job-definition',
  templateUrl: './job-definition.component.html',
  styleUrls: ['./job-definition.component.css']
})
export class JobDefinitionComponent implements OnInit {
  jobDefinitions: JobDefinitionViewModel[];
  projectId: number;
  loading: boolean;
  authorizePolicy = AuthorizePolicy;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private jobDefinitionService: JobDefinitionService,
    private jobQueueService: JobQueueService,
    private projectService: ProjectService,
    private snackbar: SnackbarService
  ) { }

  ngOnInit() {
    this.projectId = this.projectService.currentProjectId;
    this.getJobDefinitions();
  }

  getJobDefinitions() {
    this.loading = true;
    this.jobDefinitionService.getJobDefinitions(this.projectId)
      .subscribe(data => {
        this.jobDefinitions = data.map(item => ({
          selected: false,
          expanded: false,
          ...item
        }));
        this.loading = false;
      });
  }

  getJobTaskDefinitions(jobDefinition: JobDefinitionDto) {
    this.jobDefinitionService.getJobTaskDefinitions(jobDefinition.projectId, jobDefinition.id)
      .subscribe(data => {
        this.jobDefinitions = this.jobDefinitions.map(job => {
          if (job.id === jobDefinition.id) {
            job.tasks = data;
          }

          return job;
        });
      });
  }

  onNewJobDefinitionClick() {
    const dialogRef = this.dialog.open(JobDefinitionNewDialogComponent, {
      data: {
        projectId: this.projectId
      },
      minWidth: 480
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getJobDefinitions();
      }
    });
  }

  isJobDefinitionsSelected() {
    return this.jobDefinitions && this.jobDefinitions.some(m => m.selected);
  }

  onBulkDeleteClick() {
    const deletingJobDefinitions = this.jobDefinitions.filter(m => m.selected);
    const deletingJobDefinitionsString = deletingJobDefinitions.reduce((agg, item, idx) => {
      agg += `  - ${item.name}`;

      if (idx + 1 < deletingJobDefinitions.length) {
        agg += '\n';
      }

      return agg;
    }, '');

    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Job Definition',
        confirmationText: `Please enter the text "yes" to delete the following job definitions:`,
        subText: deletingJobDefinitionsString,
        confirmationMatch: 'yes'
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.jobDefinitionService.deleteJobDefinitions(this.projectId, deletingJobDefinitions.map(m => m.id))
          .subscribe(() => {
            this.snackbar.open('Job Definitions has been deleted');

            this.getJobDefinitions();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
  }

  onQueueJobClick(jobDefinition: JobDefinitionDto) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: {
        title: 'Queue Job Definition',
        confirmationText: `Are you sure you want to add ${jobDefinition.name} job to the queue?`
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.jobQueueService.addJobQueue(this.projectId, {
          projectId: this.projectId,
          jobDefinitionId: jobDefinition.id,
          jobType: null,
          originUrl: window.location.origin
        }).subscribe(data => this.router.navigateByUrl(`project/${this.projectId}/job-queue`),
          err => {
            this.dialog.open(MessageDialogComponent, {
              data: {
                title: 'Queue Job Failed',
                message: `${err}\n\nPlease make sure each task configuration is properly set.`
              }
            });
          });
      }
    });
   }


  onJobDefinitionInfoClick(jobDefinition: JobDefinitionDto) {
    const dialogRef = this.dialog.open(JobDefinitionInfoDialogComponent, {
      data: jobDefinition
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getJobDefinitions();
      }
    });
   }

  onJobDefinitionDeleteClick(job: JobDefinitionDto) {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Job Definition',
        confirmationText: `Please enter job definition name (${job.name}) to confirm deletion process:`,
        confirmationMatch: job.name
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.jobDefinitionService.deleteJobDefinition(job.projectId, job.id)
          .subscribe(() => {
            this.snackbar.open('Job Definition has been deleted');

            this.getJobDefinitions();
          }, error => {
            this.snackbar.open(error, null, {
              duration: 5000
            });
          });
      }
    });
   }

  onJobTaskDefinitionAddClick(jobDefinition: JobDefinitionDto) {
    const dialogRef = this.dialog.open(JobTaskDefinitionNewDialogComponent, {
      data: {
        projectId: this.projectId,
        jobDefinitionId: jobDefinition.id,
        jobDefinitionName: jobDefinition.name,
        isDeletion: jobDefinition.isDeletion
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.getJobTaskDefinitions(jobDefinition);
      }
    });
  }

  onCheckboxAllChanged(value: MatCheckboxChange) {
    this.jobDefinitions.forEach(m => m.selected = value.checked);
  }

  onTasksUpdated(jobDefinition: JobDefinitionDto) {
    this.getJobTaskDefinitions(jobDefinition);
  }

  onTaskExpanded(jobDefinition: JobDefinitionViewModel) {
    if (!jobDefinition.expanded) {
      this.getJobTaskDefinitions(jobDefinition);
      jobDefinition.expanded = true;
    }
  }

  onSetDefaultClick(jobDefinition: JobDefinitionDto) {
    this.jobDefinitionService.setJobDefinitionAsDefault(this.projectId, jobDefinition.id)
      .subscribe(data => {
        this.snackbar.open(`Job ${jobDefinition.name} is set to default`);
        this.getJobDefinitions();
      }, err => this.snackbar.open(err));
  }
}
