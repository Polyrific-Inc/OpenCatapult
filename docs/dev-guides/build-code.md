# Build The Source Code

Except running from the releases package, you can also run `OpenCatapult` by building from the source code. 

There are cases that make people want to do it. For example, they want to run `OpenCatapult` in a platform that the release package has not been provided. Or they want to contribute in a bug fix so they need to debug it locally. Or maybe they want to try some new features that have not been released yet.

One thing to note for sure is that this procedure might sounds too technical for average users. This is because it is actually intended to be used by someone who are familiar with basic programming knowledge. They don't need to be an ASP.NET Core programmer. As long as they are comfortable with entering some commands into a shell (PowerShell), then they are good to go.

## Pre-requisites

- [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2)
  - This is the only required tool to run build `OpenCatapult`. Other tools are optional as they are needed on some conditions only
- [Node.JS](https://nodejs.org) (optional)
  - This is required by the Web UI. You can skip this if you go all CLI.
- PowerShell (optional)
  - PowerShell is only required if you want to build the code by using the provided build scripts.
  - If you are on Windows, the PowerShell 5 should have been installed out of the box on your machine
  - If you are on Linux or Mac, you can get the cross-platform [PowerShell 6](https://docs.microsoft.com/en-us/powershell/scripting/overview?view=powershell-6)
- [Git](https://git-scm.com/) (optional)
  - This is only used to clone the source code. If you prefer to download the ZIP package instead, you don't need it
- A code editor (optional)

## Get the source code

The `OpenCatapult` source code is stored in the [GitHub](https://github.com/Polyrific-Inc/OpenCatapult) repository.

You can obtain the source code either by downloading the ZIP package or cloning it into your local machine.

Cloning the source code is simple. One way to do it is by entering this command to your favorite shell:

```sh
git clone https://github.com/Polyrific-Inc/OpenCatapult
```

Then go to the root folder:

```sh
cd OpenCatapult
```

## Build the code

From this point you have two options: build the source code using PowerShell scripts, or build manually.

### Option 1: Build using PowerShell scripts

When running the scripts, you might get execution policy error. In most of the time it can be fixed by setting the execution policy to `RemoteSigned`:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned
```

Please check the following [article](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-6) for more details about execution policies.

#### One script to rule them all

You will need to build the API, Engine, and UI (either CLI or Web) to run `OpenCatapult`. In production scenario, the components are typically installed in separated machines. However, in the development scenario, we will run them all in your local machine. We have prepared a script to help you run the components easier. Run this command in PowerShell:

```powershell
.\builds\build-all.ps1
``` 

> Note that running this script will run all 4 of the components. If you want to skip CLI or Web, you can add the `-noCli` or `-noWeb` option. For example if you want to go all CLI, use the following command instead: `.\builds\build-all.ps1 -noWeb`

The script will ask you to input the database provider and connection string. To minimize the hazzle of starting `OpenCatapult` for the first time, you can just use the default database provider, which is SQLite. It doesn't require you to provide a running SQL Server instance. However, if you prefer to use SQL Server, please enter "`mssql`" as the database provider, and enter a valid connection string, e.g.

```powershell
"Server=localhost;Database=catapult.db;User ID=sa;Password=samprod;"
```
Please enter the overriden value without quotation marks.

> Please don't close the first powershell window since it is being used to run the API, which is being used by Engine, CLI, and Web. To learn more about how opencatapult components relate to each other, please check the following [article](./intro.md#the-components).

If you want to build the API, Engine, CLI, or Web component individually, for example if you want to install them in separate machines, you can run the script for each of the component separately. Please find more details about them in the [Build Scripts](../dev-guides/build-scripts.md) section.


### Option 2: Build from source code manually

If for any reasons you cannot run the PowerShell scripts, you can always build the source code manually. Please go to the [Manually Build The Components](../dev-guides/manual-build.md) section, and go back here when finished.


## Create your first project

> *Note*: The following tutorial uses the opencatapult CLI. If you want to create the project by using Web UI, please follow this [link](../user-guides/create-first-project-web.md)

OpenCatapult allows you to define [task providers](../task-providers/task-provider.md) for all types of applications and devOps resources. We have pre-loaded OpenCatapult with an example [code generator provider](../task-providers/generator-provider.md) based on a .NET Core MVC template which will deploy directly to your local machine at this location: `.\publish\engine\working`.

To get started, open up a PowerShell window that will be used as your CLI shell and enter the following from within your OpenCatapult folder:

```sh
cd .\publish\cli\
```

### Login

When you previously applied migrations to initiate the database, a default user was created. You can use this user to login. When you're prompted to enter the password, the default password is "`opencatapult`".

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

And now, you're good to go to create a project. We will use `sample` template, which will give you some pre-defined models, and a job definition with a single `Generate` task. The task uses a built-in generator provider called `Polyrific.Catapult.TaskProviders.AspNetCoreMvc`, which will generate a starter ASP.NET Core MVC application.

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
