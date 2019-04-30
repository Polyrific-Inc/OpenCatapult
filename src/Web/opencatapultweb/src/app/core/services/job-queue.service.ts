import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { JobQueueDto } from '../models/job-queue/job-queue-dto';
import { NewJobDto } from '../models/job-queue/new-job-dto';
import { SignalrService } from './signalr.service';
import { Observable, Subject, of, BehaviorSubject } from 'rxjs';
import { JobLogDto } from '../models/job-queue/job-log-dto';
import { concatMap } from 'rxjs/operators';

@Injectable()
export class JobQueueService {
  message = new Subject<JobLogDto>();
  private currentjobs = new BehaviorSubject<JobQueueDto[]>([]);
  public get jobs(): JobQueueDto[] {
      return this.currentjobs.value;
  }

  constructor(
    private api: ApiService,
    private messageService: SignalrService) { }

  getJobQueues(projectId: number, filter: string) {
    return this.api.get<JobQueueDto[]>(`project/${projectId}/queue?filter=${filter}`)
      .pipe(concatMap((data) => {
        this.currentjobs.next(data);
        return of(data);
      }));
  }

  getJobQueue(projectId: number, queueId: number) {
    return this.api.get<JobQueueDto>(`project/${projectId}/queue/${queueId}`);
  }

  addJobQueue(projectId: number, dto: NewJobDto) {
    return this.api.post<JobQueueDto>(`project/${projectId}/queue`, dto);
  }

  updateJobQueue(projectId: number, queueId: number, dto: JobQueueDto) {
    return this.api.put(`project/${projectId}/queue/${queueId}`, dto);
  }

  getTaskLogs(projectId: number, queueId: number, taskName: string) {
    return this.api.getString(`project/${projectId}/queue/${queueId}/task/name/${taskName}/logs`);
  }

  listenJobQueueLog(projectId: number, queueId: number): Observable<JobLogDto> {
    this.messageService.connect(`jobQueueHub?projectId=${projectId}&jobQueueId=${queueId}`, this.message);
    return this.message;
  }

  disconnectJobQueueLog() {
    this.messageService.disconnect();
    this.message.complete();
    this.message = new Subject<JobLogDto>();
  }
}
