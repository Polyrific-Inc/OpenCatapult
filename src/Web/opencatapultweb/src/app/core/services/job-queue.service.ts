import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { JobDto } from '../models/job-queue/job-dto';
import { NewJobDto } from '../models/job-queue/new-job-dto';

@Injectable()
export class JobQueueService {

  constructor(private api: ApiService) { }

  getJobQueues(projectId: number, filter: string) {
    return this.api.get<JobDto[]>(`project/${projectId}/queue?filter=${filter}`);
  }

  addJobQueue(projectId: number, dto: NewJobDto) {
    return this.api.post<JobDto>(`project/${projectId}/queue`, dto);
  }
}
