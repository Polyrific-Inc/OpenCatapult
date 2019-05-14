import { JobTaskDefinitionType } from '../enums/job-task-definition-type';

export const DeletionJobTaskDefinitionTypes: [string, string][] = [
  [JobTaskDefinitionType.DeleteRepository, 'Delete Repository'],
  [JobTaskDefinitionType.DeleteHosting, 'Delete Hosting'],
  [JobTaskDefinitionType.CustomTask, 'Custom Task'],
];
