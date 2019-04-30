import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-job-queue-task-status',
  templateUrl: './job-queue-task-status.component.html',
  styleUrls: ['./job-queue-task-status.component.css']
})
export class JobQueueTaskStatusComponent implements OnInit {
  @Input() taskStatus: string;

  constructor() { }

  ngOnInit() {
  }

}
