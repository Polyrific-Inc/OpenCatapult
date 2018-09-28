# Manage job definitions

Job definitions contain jobs that can be [queued](job-queues.md) to [engine](engine-registration.md). Each job definition would contains a list of task that the engine would need to execute.

## Add job definition

Add new job definition by specifying the project name and the new job definition name
```sh
dotnet pc.dll job add --project my-project --name Default
```

All of the created job for a project can be viewed using the `list` command:
```sh
dotnet pc.dll job list --project my-project
```

## Remove job definition

Remove job definition by specifying the project name and the job definition name to be removed
```sh
dotnet pc.dll job remove --project my-project --name Default
```

## Add job task

Add a task to a job definition by specifying the project name, job definition name, and the name of the new task. You can also specify the task properties that would be used in the task's service provider
```sh
dotnet pc.dll task add --project my-project --job Default --name generate --type Generate --provider AspNetCoreMvc
```

All of the created task for a job definition can be viewed using the `list` command:
```sh
dotnet pc.dll task list --project my-project --job Default
```

## Update job task

You can update a task by specifying the project name, job definition name, and the name of the task to be updated. Then you can specify the other options to be updated:
```sh
dotnet pc.dll task update --project my-project --job Default --name generate --rename generator
```

## Remove job task

Remove a task by specifying the project name, the job definition name, and the task name to be removed
```sh
dotnet pc.dll task remove --project my-project --job Default --name generate
```

## Job task types

Available job task types:
- `Clone`: Clone the source code from the repository
- `Generate`: Generate the source code of the project
- `Push`: Upload the source code into source code repository such as Github
- `Merge`: Merge the pushed pull request into the target branch
- `Build`: Build the source code into deployable artifacts
- `PublishArtifact`: Download the build result that will be deployed
- `Deploy`: Deploy the build result into cloud provider such as Azure app service
- `DeployDb`: Apply the changes in model into the deployed database
- `Test`: Run test that's available on the project

## Built-in Providers
Following are the built-in providers. You can add other providers later using the [plugin](plugins.md) command.
- `AspNetCoreMvc`: Generate an asp net core mvc application
- `GitHubRepositoryProvider`: Used to clone or push code to GitHub
- `DotNetCoreBuildProvider`: Build a dotnet corea application
- `DotNetCoreTest`: Run tests available on the project
- `EntityFrameworkCore`: A database provider for deploying the model changes
- `AzureAppService`: Deploys the application into Azure App Service instance