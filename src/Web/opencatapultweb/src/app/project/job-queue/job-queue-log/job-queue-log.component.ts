import { Component, OnInit, ViewChild } from '@angular/core';
import { JobDto, JobQueueService } from '@app/core';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-job-queue-log',
  templateUrl: './job-queue-log.component.html',
  styleUrls: ['./job-queue-log.component.css']
})
export class JobQueueLogComponent implements OnInit {queueId: number;
  projectId: number;
  job: JobDto;
  constructor(
    private route: ActivatedRoute,
    private jobQueueService: JobQueueService,
  ) { }

  ngOnInit() {
    this.queueId = this.route.snapshot.params.id;
    this.projectId = +this.route.parent.parent.snapshot.params.id;
    this.getQueue();
  }

  getQueue() {
    this.jobQueueService.getJobQueue(this.projectId, this.queueId)
      .pipe(tap(results => {
        if (results.jobTasksStatus) {
          results.jobTasksStatus.sort((a, b) => (a.sequence > b.sequence) ? 1 : ((b.sequence > a.sequence) ? -1 : 0));
        }
      }))
      .subscribe(data => {
        this.job = data;
      });
  }

}
