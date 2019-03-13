import { JobTaskStatusDto } from './job-task-status-dto';

export interface JobQueueDto {
  id: number;
  projectId: number;
  status: string;
  catapultEngineId: string;
  jobType: string;
  catapultEngineMachineName: string;
  catapultEngineIPAddress: string;
  catapultEngineVersion: string;
  originUrl: string;
  code: string;
  jobDefinitionId: number;
  jobDefinitionName: string;
  jobTasksStatus: JobTaskStatusDto[];
  outputValues: { [key: string]: string };
  created: Date;
  remarks: string;
}
