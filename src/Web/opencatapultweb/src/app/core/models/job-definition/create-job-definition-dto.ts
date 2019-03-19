import { CreateJobTaskDefinitionDto } from './create-job-task-definition-dto';

export interface CreateJobDefinitionDto {
    name: string;
    isDeletion: boolean;
    tasks: CreateJobTaskDefinitionDto[];
}
