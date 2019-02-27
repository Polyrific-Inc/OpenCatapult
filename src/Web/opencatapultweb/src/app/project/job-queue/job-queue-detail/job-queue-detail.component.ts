import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { JobQueueService, JobDto } from '@app/core';

@Component({
  selector: 'app-job-queue-detail',
  templateUrl: './job-queue-detail.component.html',
  styleUrls: ['./job-queue-detail.component.css']
})
export class JobQueueDetailComponent implements OnInit {
  queueId: number;
  projectId: number;
  job: JobDto;
  constructor(
    private route: ActivatedRoute,
    private jobQueueService: JobQueueService
  ) { }

  ngOnInit() {
    this.queueId = this.route.snapshot.params.id;
    this.projectId = +this.route.parent.parent.snapshot.params.id;
    this.getQueue();
  }

  getQueue() {
    this.jobQueueService.getJobQueue(this.projectId, this.queueId)
      .subscribe(data => this.job = data);
  }

}
