import { Component, OnInit, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { JobTaskStatusDto, JobTaskStatus, JobQueueService, JobQueueDto } from '@app/core';
import { JobLogDto } from '@app/core/models/job-queue/job-log-dto';
import { filter, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-job-queue-task-log',
  templateUrl: './job-queue-task-log.component.html',
  styleUrls: ['./job-queue-task-log.component.css']
})
export class JobQueueTaskLogComponent implements OnInit {
  @Input() jobQueue: JobQueueDto;
  @Input() taskStatus: JobTaskStatusDto;
  currentLog: string;
  taskStatusMessages = '';
  constructor(private jobQueueService: JobQueueService) { }

  ngOnInit() {
    this.getCurrentTaskLog();
    this.jobQueueService.message
    .pipe(filter(log => log.taskName === this.taskStatus.taskName),
      map(log => log.message))
      .subscribe((message) => {
        this.taskStatusMessages += '\r\n' + message;
        const objDiv = document.getElementById(`task-log-${this.taskStatus.taskName}`);

        if (objDiv) {
          objDiv.scrollTop = objDiv.scrollHeight;
        }
      });
  }

  getCurrentTaskLog() {
    if (this.taskStatus.status !== JobTaskStatus.NotExecuted && this.taskStatus.status !== JobTaskStatus.Pending) {
      this.jobQueueService.getTaskLogs(this.jobQueue.projectId, this.jobQueue.id, this.taskStatus.taskName)
        .subscribe((log) => this.currentLog = log);
    }
  }

}
