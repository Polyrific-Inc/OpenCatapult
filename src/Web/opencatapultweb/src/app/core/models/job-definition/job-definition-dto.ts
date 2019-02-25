import { JobTaskDefinitionDto } from './job-task-definition-dto';

export interface JobDefinitionDto {
  id: number;
  name: string;
  projectId: number;
  tasks: JobTaskDefinitionDto[];
}
