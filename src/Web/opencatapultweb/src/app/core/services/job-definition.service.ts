import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { JobDefinitionDto } from '../models/job-definition/job-definition-dto';
import { JobTaskDefinitionDto } from '../models/job-definition/job-task-definition-dto';
import { CreateJobDefinitionDto } from '../models/job-definition/create-job-definition-dto';
import { UpdateTaskOrderDto } from '../models/job-definition/update-task-order-dto';
import { CreateJobTaskDefinitionDto } from '../models/job-definition/create-job-task-definition-dto';

@Injectable()
export class JobDefinitionService {

  constructor(private api: ApiService) { }

  getJobDefinitions(projectId: number) {
    return this.api.get<JobDefinitionDto[]>(`project/${projectId}/job`);
  }

  getJobTaskDefinitions(projectId: number, jobDefinitionId: number) {
    return this.api.get<JobTaskDefinitionDto[]>(`project/${projectId}/job/${jobDefinitionId}/task`);
  }

  createJobDefinition(projectId: number, jobDefinition: CreateJobDefinitionDto) {
    return this.api.post<JobDefinitionDto>(`project/${projectId}/job`, jobDefinition);
  }

  updateJobDefinition(projectId: number, jobDefinition: JobDefinitionDto) {
    return this.api.put(`project/${projectId}/job/${jobDefinition.id}`, jobDefinition);
  }

  updateTaskOrder(projectId: number, jobDefinitionId: number, dto: UpdateTaskOrderDto) {
    return this.api.put(`project/${projectId}/job/${jobDefinitionId}/task/order`, dto);
  }

  deleteJobDefinition(projectId: number, jobDefinitionId: number) {
    return this.api.delete(`project/${projectId}/job/${jobDefinitionId}`);
  }

  deleteJobDefinitions(projectId: number, jobDefinitionIds: number[]) {
    let queryParams = '';
    for (let i = 0; i < jobDefinitionIds.length; i++) {
      queryParams += `jobIds=${jobDefinitionIds[i]}`;

      if (i + 1 < jobDefinitionIds.length) {
        queryParams += '&';
      }
    }

    return this.api.delete(`project/${projectId}/job/bulkdelete?${queryParams}`);
  }

  deleteJobTaskDefinition(projectId: number, jobDefinitionId: number, jobTaskDefinitionId: number) {
    return this.api.delete(`project/${projectId}/job/${jobDefinitionId}/task/${jobTaskDefinitionId}`);
  }

  updateJobTaskDefinition(
    projectId: number, jobDefinitionId: number,
    jobTaskDefinitionId: number, jobTaskDefinition: JobTaskDefinitionDto) {
    return this.api.put(`project/${projectId}/job/${jobDefinitionId}/task/${jobTaskDefinitionId}`, jobTaskDefinition);
  }

  createJobTaskDefinition(projectId: number, jobDefinitionId: number, jobTaskDefinition: CreateJobTaskDefinitionDto) {
    return this.api.post<JobTaskDefinitionDto>(`project/${projectId}/job/${jobDefinitionId}/task`, jobTaskDefinition);
  }
}
