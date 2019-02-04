# Quick Start

This document will guide you to get started with `OpenCatapult`.

## Setup from release library

[coming soon]

## Build from source code

### Pre-requisites

- Git (https://git-scm.com/)
- .Net Core 2.1 SDK (https://dotnet.microsoft.com/download/dotnet-core/2.1)
  - `OpenCatapult` requires the LTS version as minimum requirement (which is SDK 2.1.500)
- SQL Server 2017 (https://www.microsoft.com/en-us/sql-server/sql-server-2017)
  - If you want to use local instance, you can use the free `Express` or `Developer` edition
  - You can alo use remote instance (e.g. Azure SQL)
- [Optional] Code editor, e.g. Visual Studio Code (https://code.visualstudio.com/)

### Get the source code

Clone `OpenCatapult` source code from the repository:

```sh
git clone https://github.com/Polyrific-Inc/OpenCatapult.git
```

Go to the root folder:

```sh
cd OpenCatapult
```

From this point you have two options: build the source code using PowerShell scripts, or build manually.

### Option 1: Build using PowerShell scripts

> *Pro tips*: did you know that you can install PowerShell in Mac and Linux? Please check this out: <https://docs.microsoft.com/en-us/powershell/scripting/overview>

When running the scripts, you might get execution policy error. In most of the time it can be fixed by setting the execution policy to `RemoteSigned`:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned
```

Please check the following [article](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-6) for more details about execution policies.

#### One script to rule them all

You will need to build the API, CLI, and Engine application to run opencatapult. In production scenario, all three are typically installed in separated machines. However, for this quick start, we will run them all in your local machine. We have prepared the script that will run the build script for API, CLI, and Engine. Run this command in the powershell:
```powershell
.\builds\build-all.ps1
``` 

The script will ask you to input the connection string for the API. After the API build succeed, it will open two new powershell windows. One is for Engine and one is for CLI.

> Please don't close the first powershell window since it is being used to run the API, which is being used by CLI and Engine. To learn more about how opencatapult components relate to each other, please check the following [article](./intro.md#the-components).

If you want to build the API, CLI, or Engine one by one, for example if you want to install them in separate machines, you can follow the guide [here](../user-guides/build-scripts.md).


### Option 2: Build from source code manually

If for any reasons you cannot run the PowerShell scripts, you can always build the source code manually by following these steps:

> Note: the location paths use backslash (`\`) here, which is the Windows style. If you work on Mac or Linux environment, you might need to replace it with slash (`/`).

#### Setup database

Please make sure you have access to a SQL Server instance, and put the connection string in `.\src\API\Polyrific.Catapult.Api\appsettings.json`, e.g.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=opencatapult-db;User ID=sa;Password=sapassword;"
  }
}
```

> Note: You don't need to create the concrete database in advance because EF will automatically create it if it doesn't exist. Please make sure though that the user which is defined in connection string has "database create" permission.

Run the migration script to initialize the database:

```sh
dotnet ef database update --startup-project .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj --project .\src\API\Polyrific.Catapult.Api.Data\Polyrific.Catapult.Api.Data.csproj
```

#### Run the API

Let's publish and run the API in localhost:

```sh
dotnet publish .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj -c Release -o ..\..\..\publish\api
cd .\publish\api\
dotnet .\ocapi.dll --urls "http://localhost:8005;https://localhost:44305"
```

#### Prepare the Engine

Open new shell, go to the root folder, and publish the Engine:

```sh
dotnet publish .\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj -c Release -o ..\..\..\publish\engine
```

Set API URL in the Engine's config:

```sh
dotnet .\publish\engine\ocengine.dll config set -n ApiUrl -v https://localhost:44305
```

While configuring the Engine environment, let's publish the built-in Task Providers as well. They will be required later when executing job tasks.

```sh
dotnet publish .\src\Plugins\GeneratorProvider\Polyrific.Catapult.Plugins.AspNetCoreMvc\src\Polyrific.Catapult.Plugins.AspNetCoreMvc.csproj -c Release -o ..\..\..\..\publish\engine\plugins\GeneratorProvider\Polyrific.Catapult.Plugins.AspNetCoreMvc
dotnet publish .\src\Plugins\HostingProvider\Polyrific.Catapult.Plugins.AzureAppService\src\Polyrific.Catapult.Plugins.AzureAppService.csproj -c Release -o ..\..\..\..\publish\engine\plugins\HostingProvider\Polyrific.Catapult.Plugins.AzureAppService
dotnet publish .\src\Plugins\BuildProvider\Polyrific.Catapult.Plugins.DotNetCore\src\Polyrific.Catapult.Plugins.DotNetCore.csproj -c Release -o ..\..\..\..\publish\engine\plugins\BuildProvider\Polyrific.Catapult.Plugins.DotNetCore
dotnet publish .\src\Plugins\TestProvider\Polyrific.Catapult.Plugins.DotNetCoreTest\src\Polyrific.Catapult.Plugins.DotNetCoreTest.csproj -c Release -o ..\..\..\..\publish\engine\plugins\TestProvider\Polyrific.Catapult.Plugins.DotNetCoreTest
dotnet publish .\src\Plugins\DatabaseProvider\Polyrific.Catapult.Plugins.EntityFrameworkCore\src\Polyrific.Catapult.Plugins.EntityFrameworkCore.csproj -c Release -o ..\..\..\..\publish\engine\plugins\DatabaseProvider\Polyrific.Catapult.Plugins.EntityFrameworkCore
dotnet publish .\src\Plugins\RepositoryProvider\Polyrific.Catapult.Plugins.GitHub\src\Polyrific.Catapult.Plugins.GitHub.csproj -c Release -o ..\..\..\..\publish\engine\plugins\RepositoryProvider\Polyrific.Catapult.Plugins.GitHub
```

> Note: There is one more steps required to start the Engine, which is to enter the authorization token. But we will do it later after registering the Engine via CLI.

#### Prepare the CLI

Open new shell, go to the root folder, and build the CLI project:

```sh
dotnet publish .\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj -c Release -o ..\..\..\publish\cli
```

Set API URL in the CLI's config:

```sh
dotnet .\publish\cli\occli.dll config set -n ApiUrl -v https://localhost:44305
```

**Note:**

If you have some error related to ssl, you can try to run the following command, and accept the popup prompt:

```sh
dotnet dev-certs https --trust
```

If for any reasons you cannot use https, you can just use the http url. But please make sure you are not in `Production` environment, otherwise you will be redirected to the https url.
Setting up `Development` environment can be done via environment variable:

```sh
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

You are now ready to create your first Catapult project.

## Create your first project

OpenCatapult allows you to define [task providers](../task-providers/task-provider.md) for all types of applications and devOps resources. We have pre-loaded OpenCatapult with an example [code generator provider](../task-providers/generator-provider.md) based on a .NET Core MVC template which will deploy directly to your local machine at this location: `.\publish\engine\working`.

To get started, open up a PowerShell window that will be used as your CLI shell and enter the following from within your OpenCatapult folder:

```sh
cd .\publish\cli\
```

### Login

When you previously applied migrations to initiate the database, a default user was created. You can use this user to login. When you're prompted to enter the password, the default password is `opencatapult`.

```sh
dotnet occli.dll login --user admin@opencatapult.net
```

We strongly advise you to change the default password (or just remove the default user), especially when you deploy the API into public environment:

```sh
dotnet occli.dll account password update
```

### Register and Start the Engine

We need to register Engine so it can communicate with the API without problem. It involves step to register the Engine via CLI, and enter the generated token back in the Engine itself. If you have multiple Engine instances, you need to do this procedure on each of them.

Activate the CLI shell, and enter this command:

```sh
dotnet occli.dll engine register --name Engine001
```

After the Engine is registered, let's generate a token for it, and copy the generated token:

```sh
dotnet occli.dll engine token --name Engine001
```

Activate the Engine shell, go to the engine's published folder, and set the `AuthorizationToken` config with the copied generated token:

```sh
cd .\publish\engine\
dotnet ocengine.dll config set --name AuthorizationToken --value [the-generated-token]
```

Let's start the Engine:

```sh
dotnet ocengine.dll start
```

You can find more details about these procedure at [Manage engine registration](../user-guides/engine-registration.md)

### Create sample project

And now, you're good to go to create a project. We will use `sample` template, which will give you some pre-defined models, and a job definition with a single `Generate` task. The task uses a built-in generator provider called `Polyrific.Catapult.Plugins.AspNetCoreMvc`, which will generate a starter ASP.NET Core MVC application.

Activate the CLI shell, and enter this command:

```sh
dotnet occli.dll project create --name first-project --client Polyrific --template sample
```

During the process you will be prompted to enter `Admin Email`. Please fill it with your email.

After the project is created you can check the created elements in it. For example you might want to check the created data models:

```sh
dotnet occli.dll model list -p first-project
```

### Queue the job

As explained in [introduction](./intro.md#the-circle-of-magic), the automation logics happens in the engine. We define what the engine shall do in what we call [job](../user-guides/job-definitions.md). A job can contain many tasks as needed by our development, build, and deployment pipeline. When we want to ask the engine to run a job, we add it into the [queue](../user-guides/job-queues.md), and any active engine will pick it up and execute it.

The project that you've just created contains a `Default` job definition with a `Generate` task in it.

```sh
dotnet occli.dll job get -n Default -p first-project
```

Let's add the job to the queue so Engine can pick and execute it.

```sh
dotnet occli.dll queue add -j Default -p first-project
```

 The above command will print the created queue in the CLI. You should check out the `Id` since this will be used to get the log or queue status. If this is your first time adding queue, the `Id` should be 1.
 
![Queue add](../img/queue-add.JPG)

You can monitor the live progress by using the `queue log` command and passing the queue `Id`:

```sh
dotnet occli.dll queue log -n 1 -p first-project
```

The final status of the process can be checked by this command:

```sh
dotnet occli.dll queue get -n 1 -p first-project
```

It will tell you the status of each task execution, whether it's Success or Failed, along with the error remarks if any.

## Next steps

After creating your first project, you can:

- [add models to the project](../user-guides/data-models.md)
- [explore what else you can do with the project](../user-guides/user-guides.md)
- [create another project by using Sample-DevOps project template](../user-guides/sample-project.md)
- check references of [API](../api/api.md), [Engine](../engine/engine.md), [CLI](../cli/cli.md)