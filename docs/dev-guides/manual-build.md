# Manually Build The Components

In [Quick Start](../home/start.md), we have shown you the way to easily build `OpenCatapult` components by using scripts. However, there are times which propbably make you want to build the source code manually. This guideline will show you the required steps on how to do it.

> Note: In this docs, the location paths are decorated with backslash ( `\` ), which is the Windows style. If you work on Mac or Linux environment, you might need to replace it with slash ( `/` ).

## Setup database

First, let's setup the database. Please make sure you have access to a SQL Server instance, and put the connection string to app settings file, e.g. `.\src\API\Polyrific.Catapult.Api\appsettings.json`. You could also put it in the environment specific settings file, e.g. `appsettings.Development.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=opencatapult-db;User ID=sa;Password=sapassword;"
  }
}
```

> Note: If you target local database server, you don't need to create the concrete database in advance because EF will automatically create it if it doesn't exist. Please make sure though that the user which is defined in connection string has "database create" permission.

Run the migration script to initialize the database:

```sh
dotnet ef database update --startup-project .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj --project .\src\API\Polyrific.Catapult.Api.Data\Polyrific.Catapult.Api.Data.csproj
```

## Run the API

Let's publish and run the API in localhost:

```sh
dotnet publish .\src\API\Polyrific.Catapult.Api\Polyrific.Catapult.Api.csproj -c Release -o ..\..\..\publish\api
cd .\publish\api\
dotnet .\ocapi.dll --urls "http://localhost:8005;https://localhost:44305"
```

You could change the defined URLs, but please take a note of it because it will be used by other components as well to connect to this API.

**Note:**

If you have some errors related to SSL, you can try to run the following command, and accept the popup prompt:

```sh
dotnet dev-certs https --trust
```

If for any reasons you cannot use https, you can just use the http url. But please make sure you are not in `Production` environment, otherwise you will be redirected to the https url.
Setting up `Development` environment can be done via environment variable:

```sh
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

## Prepare the Engine

Open new shell, go to the root folder, and publish the Engine:

```sh
dotnet publish .\src\Engine\Polyrific.Catapult.Engine\Polyrific.Catapult.Engine.csproj -c Release -o ..\..\..\publish\engine
```

When finished, you will get a ready to execute Engine component. To see if it works, try to set the API URL in the Engine's config by using the built-in command:

```sh
dotnet .\publish\engine\ocengine.dll config set -n ApiUrl -v https://localhost:44305
```

While configuring the Engine environment, let's publish the built-in Task Providers as well. They will be required later when executing job tasks.
While this is optional, if you are not sure that you will need them, we recommend to just build them all. FYI, each of these Task Providers will be required if you create a project with `sample-devops` template later.

```sh
dotnet publish .\src\Plugins\GeneratorProvider\Polyrific.Catapult.TaskProviders.AspNetCoreMvc\src\Polyrific.Catapult.TaskProviders.AspNetCoreMvc.csproj -c Release -o ..\..\..\..\publish\engine\plugins\GeneratorProvider\Polyrific.Catapult.TaskProviders.AspNetCoreMvc
dotnet publish .\src\Plugins\HostingProvider\Polyrific.Catapult.TaskProviders.AzureAppService\src\Polyrific.Catapult.TaskProviders.AzureAppService.csproj -c Release -o ..\..\..\..\publish\engine\plugins\HostingProvider\Polyrific.Catapult.TaskProviders.AzureAppService
dotnet publish .\src\Plugins\BuildProvider\Polyrific.Catapult.TaskProviders.DotNetCore\src\Polyrific.Catapult.TaskProviders.DotNetCore.csproj -c Release -o ..\..\..\..\publish\engine\plugins\BuildProvider\Polyrific.Catapult.TaskProviders.DotNetCore
dotnet publish .\src\Plugins\TestProvider\Polyrific.Catapult.TaskProviders.DotNetCoreTest\src\Polyrific.Catapult.TaskProviders.DotNetCoreTest.csproj -c Release -o ..\..\..\..\publish\engine\plugins\TestProvider\Polyrific.Catapult.TaskProviders.DotNetCoreTest
dotnet publish .\src\Plugins\DatabaseProvider\Polyrific.Catapult.TaskProviders.EntityFrameworkCore\src\Polyrific.Catapult.TaskProviders.EntityFrameworkCore.csproj -c Release -o ..\..\..\..\publish\engine\plugins\DatabaseProvider\Polyrific.Catapult.TaskProviders.EntityFrameworkCore
dotnet publish .\src\Plugins\RepositoryProvider\Polyrific.Catapult.TaskProviders.GitHub\src\Polyrific.Catapult.TaskProviders.GitHub.csproj -c Release -o ..\..\..\..\publish\engine\plugins\RepositoryProvider\Polyrific.Catapult.TaskProviders.GitHub
```

> Note: There is one more steps required before being able to start the Engine, which is to enter the authorization token. But we will do it later after registering the Engine via CLI.

## Prepare the CLI

If you want to use `Command Line Interface` as your front-end interface to interact with `OpenCatapult`, let's build the CLI component.

Open new shell, go to the root folder, and build the CLI project:

```sh
dotnet publish .\src\CLI\Polyrific.Catapult.Cli\Polyrific.Catapult.Cli.csproj -c Release -o ..\..\..\publish\cli
```

When finished, you will get a CLI component that is ready to accept any commands you enter to interact with `OpenCatapult`. To see if it works fine, let's try to set the API URL in the config by using the built-in command:

```sh
dotnet .\publish\cli\occli.dll config set -n ApiUrl -v https://localhost:44305
```

## Run the Web UI

If you prefer to use Web UI to interact with `OpenCatapult`, you need to build and run it. It requires you to have [Node,js](https://nodejs.org) installed on your machine. If you don't have it, please download and install it first, and go back here to continue to run the Web UI component.

Open a new shell, go to the root folder, and go to the web project directory:

```sh
cd .\src\Web\opencatapultweb
```

Run the following commands:

```sh
npm install
npm run start -- --ssl --host localhost --port 44300 --ssl-cert "ssl/server.crt" --ssl-key "ssl/server.key"
```

The command will run the Web UI in https://localhost:44300, but feel free to use different URL if you want to.

As it uses SSL protocol, you need to provide SSL certificate when running it. There are many tutorials in internet on how to generate local certificate for development purpose. If you don't have time to do it, `OpenCatapult` has provided a dummy certificate which you can use to quickly start the web. But you still need to trust it first so your browser won't complain.

The dummy certificate is located in `.\src\Web\opencatapultweb\ssl\server.crt`.

As all components have been built and run, your `OpenCatapult` ecosystem is ready now. If you came here from the `Quick Start` tutorial, it is the time to go back [there](../home/start.md#create-your-first-project) and continue to create your first `OpenCatapult` project.