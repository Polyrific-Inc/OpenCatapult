# Database Provider

Database providers can be used in [Deploy Db task](../user-guides/job-definitions.md#deploydb). The main role of this provider is to provide specific way to update database in the deployment environment.

## Polyrific.Catapult.Plugins.EntityFrameworkCore

`OpenCatapult` provides `Polyrific.Catapult.Plugins.EntityFrameworkCore` as the built-in provider for Database Provider. This provider can be used to update a specified database to the latest migration script available in the source code. It uses the `dotnet ef database update` command that comes with the dotnet core entity framework sdk.

### Usage

This provider can only be used in DeployDb task. You can use the name `Polyrific.Catapult.Plugins.EntityFrameworkCore` when adding or updating a DeployDb task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n DeployDb -t DeployDb -prov Polyrific.Catapult.Plugins.EntityFrameworkCore
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n DeployDb -prov Polyrific.Catapult.Plugins.EntityFrameworkCore
```

### Additional configs

This provider has several additional configurations that you can use to fit your use case:

| Name | Description | Default Value | Mandatory |
| --- | --- | --- | --- | --- |
| StartupProjectName | The name of the startup project | {ProjectName}| No |
| DatabaseProjectName | The name of the project that contains EF database migrations | {ProjectName}.Data| No |
| ConnectionString | The connection string to the database that will be updated | -| Yes |