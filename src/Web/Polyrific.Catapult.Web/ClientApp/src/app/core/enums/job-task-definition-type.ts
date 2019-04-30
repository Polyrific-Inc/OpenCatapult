export enum JobTaskDefinitionType {
    Build = 'Build',
    Pull = 'Pull',
    Deploy = 'Deploy',
    DeployDb = 'DeployDb',
    Generate = 'Generate',
    Merge = 'Merge',
    Push = 'Push',
    PublishArtifact = 'PublishArtifact',
    Test = 'Test',
    DeleteRepository = 'DeleteRepository',
    DeleteHosting = 'DeleteHosting',
    CustomTask = 'CustomTask'
}
