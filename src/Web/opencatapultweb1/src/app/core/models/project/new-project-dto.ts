import { NewProjectMemberDto } from '../member/new-project-member-dto';
import { CreateDataModelDto } from '../data-model/create-data-model-dto';
import { CreateJobDefinitionDto } from '../job-definition/create-job-definition-dto';

export interface NewProjectDto {
    name: string;
    displayName: string;
    client: string;
    members: NewProjectMemberDto[];
    models: CreateDataModelDto[];
    jobs: CreateJobDefinitionDto[];
}
