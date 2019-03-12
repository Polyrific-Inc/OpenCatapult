# Manage job definitions

Job definitions contain jobs that can be [queued](job-queues.md) to [engine](engine-registration.md). Each job definition would contains a list of task that the engine would need to execute.

## Add job definition

Add new job definition by specifying the project name and the new job definition name
```sh
dotnet occli.dll job add --project MyProject --name Default
```

All of the created job for a project can be viewed using the `list` command:
```sh
dotnet occli.dll job list --project MyProject
```

## Remove job definition

Remove job definition by specifying the project name and the job definition name to be removed
```sh
dotnet occli.dll job remove --project MyProject --name Default
```

## Add job task

Add a task to a job definition by specifying the project name, job definition name, and the name of the new task. You can also specify the task properties that would be used in the task's service provider
```sh
dotnet occli.dll task add --project MyProject --job Default --name generate --type Generate --provider AspNetCoreMvc
```

Each job task type will have specific [configurations](#job-task-types) that will be prompted for input. You can leave them blank if it's not required. [Generic configurations](#generic-task-configurations) will not be prompted for input. Instead, you can set them using the `--property` option, and specify the configuration names and their values, separated by `:`. For example this will set the `ContinueWhenError` configuration:
```sh
dotnet occli.dll task add --project MyProject --job Default --name generate --type Generate --provider AspNetCoreMvc --property ContinueWhenError:true
```

All of the created task for a job definition can be viewed using the `list` command:
```sh
dotnet occli.dll task list --project MyProject --job Default
```

## Update job task

You can update a task by specifying the project name, job definition name, and the name of the task to be updated. Then you can specify the other options to be updated:
```sh
dotnet occli.dll task update --project MyProject --job Default --name generate --rename generator
```

To update the task [configurations](#job-task-types), you can use the `--property` option, and specify the configuration names and their values, separated by `:`. For example this will set the `OutputLocation` configuration of the `Generate` task:
```sh
dotnet occli.dll task update --project MyProject --job Default --name generate --property OutputLocation:C:\GeneratedCode
```

## Remove job task

Remove a task by specifying the project name, the job definition name, and the task name to be removed
```sh
dotnet occli.dll task remove --project MyProject --job Default --name generate
```

## Job task types

Each job task type have a specific function, and its own configurations.

### Clone
Clone the source code from the repository.

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| Repository | The remote url of the repository | - | - | Yes |
| IsPrivateRepository | Indicates whether the repository is private or public | true, false | false | Yes |
| CloneLocation | The directory path of the cloned repository. Leave blank to use default directory from engine | - | - | No |
| BaseBranch | The branch you want to be working on | - | - | No |

### Generate
Generate the source code of the project.

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| OutputLocation | The directory path of the generated code. Leave blank to use default directory from engine | - | - | No |

### Push
Upload the source code into source code repository such as Github

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| Repository | The remote url of the repository | - | - | Yes |
| SourceLocation | The directory path of repository to be pushed. Leave blank to use default directory from engine | - | - | No |
| Branch | The branch name | - | - | No |
| CreatePullRequest | Indicates whether to create a pull request after the changes has been pushed to the branch | true, false | false | No |
| PullRequestTargetBranch | The target branch of the pull request to be merged | - | - | No |
| CommitMessage | The commit message  | - | - | No |
| Author | The commit author's name | - | - | No |
| Email | The commit author's email | - | - | No |

### Merge
Merge the pushed pull request into the target branch

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| Repository | The remote url of the repository | - | - | Yes |

### Build
Build the source code into deployable artifacts

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| SourceLocation | The directory path of the generated code | - | - | No |
| OutputArtifactLocation | The directory path of the build output artifact. Leave blank to use default directory from engine | - | - | No |

### PublishArtifact
Download the build result that will be deployed

### Deploy
Deploy the build result into cloud provider such as Azure app service

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| ArtifactLocation | The directory path of the artifact to be deployed. If left blank, it'll look into the default output artifact location of the build task | - | - | No |

### DeployDb
Apply the changes in model into the deployed database

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| MigrationLocation | The directory path of published source code that will be used to do db migration. If left blank, it'll look into the default output publish location of the build task or publish artifact task | - | - | No |

### Test
Run test that's available on the project

| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| TestLocation | The directory path of test to be run. Leave blank to use default directory from engine | - | - | No |
| ContinueWhenFailed | Indicates whether to continue the task execution even when the test result is failed | true, false | false | No |

### Generic task configurations
For all of the Task types mentioned, you can also specify the following generic configurations that apply to all of them
| Config Name | Description | Allowed Values | Default Value | Mandatory |
| ---  | --- | --- | --- | --- |
| ContinueWhenError | Indicates whether to continue the task execution even when the task throws error | true, false | false | No |
| PreProcessMustSucceed | Indicates whether the pre-process needs to be success before executing the main task | true, false | true | No |
| PostProcessMustSucceed | Indicates whether the post-process needs to be success to complete the task | true, false | false | No |
| WorkingLocation | The directory path where the engine will be working on and output the result | - | - | No |

## Built-in Providers
Following are the built-in providers. You can add other providers later using the [task provider](task-providers.md) command.
- `Polyrific.Catapult.TaskProviders.AspNetCoreMvc`: Generate an asp net core mvc application
- `Polyrific.Catapult.TaskProviders.GitHub`: Used to clone or push code to GitHub
- `Polyrific.Catapult.TaskProviders.DotNetCore`: Build a dotnet core application
- `Polyrific.Catapult.TaskProviders.DotNetCoreTest`: Run tests available on the project
- `Polyrific.Catapult.TaskProviders.EntityFrameworkCore`: A database provider for deploying the model changes
- `Polyrific.Catapult.TaskProviders.AzureAppService`: Deploys the application into Azure App Service instance