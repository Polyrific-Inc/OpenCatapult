import { JobTaskDefinitionType } from '../enums/job-task-definition-type';

export const jobTaskDefinitionTypes: [string, string][] = [
  [JobTaskDefinitionType.Clone, 'Clone'],
  [JobTaskDefinitionType.Generate, 'Generate'],
  [JobTaskDefinitionType.Push, 'Push'],
  [JobTaskDefinitionType.Merge, 'Merge'],
  [JobTaskDefinitionType.Build, 'Build'],
  [JobTaskDefinitionType.DeployDb, 'Deploy Db'],
  [JobTaskDefinitionType.Deploy, 'Deploy'],
  [JobTaskDefinitionType.PublishArtifact, 'Publish Artifact'],
  [JobTaskDefinitionType.Test, 'Test'],
];
