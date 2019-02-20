import { CreateJobTaskDefinitionDto } from './create-job-task-definition-dto';

export interface CreateJobDefinitionDto {
    name: string;
    tasks: CreateJobTaskDefinitionDto[];
}
