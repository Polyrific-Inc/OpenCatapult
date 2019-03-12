# Build Provider

Build providers can be used in [Build task](../user-guides/job-definitions.md#build). The main role of this provider is to provide specific way to build the generated code, and produce a deliverable artifact. The artifact can then be used as input for [Hosting provider](hosting-provider.md) or [Database provider](database-provider.md).

## Polyrific.Catapult.TaskProviders.DotNetCore

`OpenCatapult` provides `Polyrific.Catapult.TaskProviders.DotNetCore` as the built-in provider for Build Provider. This provider can be used to build and create deployable artifact for dotnet core applications. It uses the `dotnet publish` command that comes with the dotnet core sdk.

### Usage

This provider can only be used in Build task. You can use the name `Polyrific.Catapult.TaskProviders.DotNetCore` when adding or updating a Build task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Build -t Build -prov Polyrific.Catapult.TaskProviders.DotNetCore
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Build -prov Polyrific.Catapult.TaskProviders.DotNetCore
```

### Additional configs

This provider has several additional configurations that you can use to fit your use case:

| Name | Description | Default Value | Mandatory |
| --- | --- | --- | --- | --- |
| CsprojLocation | The location/path of the startup project | {ProjectName}.csproj| No |
| Configuration | The build configuration used in the build process | Release| No |