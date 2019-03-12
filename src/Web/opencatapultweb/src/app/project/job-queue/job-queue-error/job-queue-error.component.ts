import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-job-queue-error',
  templateUrl: './job-queue-error.component.html',
  styleUrls: ['./job-queue-error.component.css']
})
export class JobQueueErrorComponent implements OnInit {
  projectId: number;
  jobQueueId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.projectId = +this.route.parent.parent.snapshot.params.projectId;
    this.jobQueueId = this.route.snapshot.params.id;
  }

}
