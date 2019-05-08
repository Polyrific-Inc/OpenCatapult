import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-job-queue-status',
  templateUrl: './job-queue-status.component.html',
  styleUrls: ['./job-queue-status.component.css']
})
export class JobQueueStatusComponent implements OnInit {
  @Input() status: string;

  constructor() { }

  ngOnInit() {
  }

}
