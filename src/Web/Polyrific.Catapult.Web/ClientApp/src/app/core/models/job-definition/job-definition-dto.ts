import { JobTaskDefinitionDto } from './job-task-definition-dto';

export interface JobDefinitionDto {
  id: number;
  name: string;
  projectId: number;
  isDeletion: boolean;
  isDefault: boolean;
  tasks: JobTaskDefinitionDto[];
}
