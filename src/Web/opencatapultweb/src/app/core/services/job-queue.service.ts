import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { JobDto } from '../models/job-queue/job-dto';
import { NewJobDto } from '../models/job-queue/new-job-dto';
import { SignalrService } from './signalr.service';
import { Observable, Subject } from 'rxjs';
import { JobLogDto } from '../models/job-queue/job-log-dto';

@Injectable()
export class JobQueueService {
  message = new Subject<JobLogDto>();

  constructor(
    private api: ApiService,
    private messageService: SignalrService) { }

  getJobQueues(projectId: number, filter: string) {
    return this.api.get<JobDto[]>(`project/${projectId}/queue?filter=${filter}`);
  }

  getJobQueue(projectId: number, queueId: number) {
    return this.api.get<JobDto>(`project/${projectId}/queue/${queueId}`);
  }

  addJobQueue(projectId: number, dto: NewJobDto) {
    return this.api.post<JobDto>(`project/${projectId}/queue`, dto);
  }

  updateJobQueue(projectId: number, queueId: number, dto: JobDto) {
    return this.api.put(`project/${projectId}/queue/${queueId}`, dto);
  }

  getTaskLogs(projectId: number, queueId: number, taskName: string) {
    return this.api.getString(`project/${projectId}/queue/${queueId}/task/name/${taskName}/logs`);
  }

  listenJobQueueLog(projectId: number, queueId: number): Observable<JobLogDto> {
    this.messageService.connect(`jobQueueHub?projectId=${projectId}&jobQueueId=${queueId}`, this.message);
    return this.message;
  }
}
