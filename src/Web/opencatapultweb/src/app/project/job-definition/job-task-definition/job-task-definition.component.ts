import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { JobTaskDefinitionDto, JobDefinitionDto, JobDefinitionService } from '@app/core';
import { MatDialog } from '@angular/material';
import { SnackbarService, ConfirmationWithInputDialogComponent } from '@app/shared';
import {
  JobTaskDefinitionInfoDialogComponent
} from '../components/job-task-definition-info-dialog/job-task-definition-info-dialog.component';

@Component({
  selector: 'app-job-task-definition',
  templateUrl: './job-task-definition.component.html',
  styleUrls: ['./job-task-definition.component.css']
})
export class JobTaskDefinitionComponent implements OnInit {
  @Input() jobTaskDefinitions: JobTaskDefinitionDto[];
  @Input() jobDefinition: JobDefinitionDto;
  @Output() tasksChanged = new EventEmitter<JobDefinitionDto>();
  constructor(
    private dialog: MatDialog,
    private jobDefinitionService: JobDefinitionService,
    private snackbar: SnackbarService
    ) { }

  ngOnInit() {
  }

  onTaskInfoClick(task: JobTaskDefinitionDto) {
    const dialogRef = this.dialog.open(JobTaskDefinitionInfoDialogComponent, {
      data: {
        projectId: this.jobDefinition.projectId,
        jobDefinitionName: this.jobDefinition.name,
        ...task
      }
    });

    dialogRef.afterClosed().subscribe(success => {
      if (success) {
        this.tasksChanged.emit(this.jobDefinition);
      }
    });
  }

  onTaskDeleteClick(task: JobTaskDefinitionDto) {
    const dialogRef = this.dialog.open(ConfirmationWithInputDialogComponent, {
      data: {
        title: 'Confirm Delete Job Task Definition',
        confirmationText: 'Please enter job task definition name to confirm deletion process:',
        confirmationMatch: task.name
      }
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.jobDefinitionService.deleteJobTaskDefinition(this.jobDefinition.projectId, task.jobDefinitionId, task.id)
          .subscribe(() => {
            this.snackbar.open('Job task definition has been deleted');

            this.tasksChanged.emit(this.jobDefinition);
          });
      }
    });
  }

}
