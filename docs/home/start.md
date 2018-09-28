# Quick Start

This document will guide you to get started with `OpenCatapult`.

## Setup from release library

Please go to [release page](https://opencatapult.net/releases) to download the latest version of `OpenCatapult`.

Unzip the release package into your local directory, and run the components.

## Build from source code

Clone `OpenCatapult` source code from the repository:

```sh
git clone https://github.com/Polyrific-Inc/OpenCatapult.git
```

Go to the root folder:

```sh
cd OpenCatapult
```

Modify the connection string in `.\src\API\Polyrific.Catapult.Api\appsettings.json` to connect to your database server

Run the migration script to initialize the database:
```sh
dotnet ef database update --startup-project .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj --project .\src\API\Polyrific.Catapult.Api.Data\Polyrific.Catapult.Api.Data.csproj
```

Run the API:

```sh
dotnet run -p .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj -c Release
```

Open new shell, go to the root folder, build and set the Api Url for the engine:

```sh
dotnet build .\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj -c Release
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll config set -n ApiUrl -v https://localhost:5001
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll --help
```

Open new shell, go to the root folder, build and run a CLI command:

```sh
dotnet build .\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj -c Release
dotnet .\src\CLI\Polyrific.Catapult.Cli\bin\Release\PC.dll config set -n ApiUrl -v https://localhost:5001
dotnet .\src\CLI\Polyrific.Catapult.Cli\bin\Release\PC.dll --help
```

You are now ready to create your first catapult project.

Note:

If you have some error related to ssl, you can try to run the following command, and accept the popup prompt:
```sh
dotnet dev-certs https --trust
```

## Create your first project

To make further command shorter, go to the release folder of the catapult CLI:
```sh
cd src\CLI\Polyrific.Catapult.Cli\bin\Release
```

The first thing to do is to login into the API. During instalation above, a default admin user is created in the database:
```sh
dotnet PC.dll login --user admin@opencatapult.net
```
Then enter the password: `opencatapult`

We strongly advise you to change your password using the `account updateuser` command:
```sh
dotnet PC.dll account updateuser --email admin@opencatapult.net
```

Before adding the project, we need to prepare the external service that will be used by the project. For our sample project, it would need a `GitHub` and `AzureAppService` external service
```sh
dotnet PC.dll service add --name github-default --type GitHub
```
Then you will be prompted to enter the required details for `GitHub`

```sh
dotnet PC.dll service add --name azure-default --type AzureAppService
```
Then you will be prompted to enter the required details for `AzureAppService`


We can then create our first project using the `project create` command. We will use a sample template that is provided out of the box, to make the demo easier
```sh
dotnet PC.dll project create --name my-project --client Polyrific --template sample
```

You will be prompted to enter some additional configuration such as Azure subscription id, resource group, etc. Once it's done, your project will be created and we're ready to run the job to generate and deploy our applications.

To run the job, use the `queue` command:
```sh
dotnet PC.dll queue add --project my-project --job Default
```

We have succesfully queue our project to be processed by engine.

But first, we need to register a new engine instance, and get the engine access token so it can communicate to the API:
```sh
dotnet PC.dll engine register --name Engine01
dotnet PC.dll engine token --name Engine01
```

Copy the engine token, then open a new CLI and go to the catapult directory. Run the following command to set authorization token of the engine:
```sh
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll config set -n AuthorizationToken -v <paste the token here>
```

Finally, start the engine so that it will process our queued job:
```sh
dotnet .\src\Engine\Polyrific.Catapult.Engine\bin\Release\PCEngine.dll start
```
