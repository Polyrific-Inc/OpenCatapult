# Test Provider

Test Providers can be used in [Test task](../user-guides/job-definitions.md#test). The main role of this provider is to provide a specific way to run a test based on the selected test runner.

## Polyrific.Catapult.TaskProviders.DotNetCoreTest

`OpenCatapult` provides `Polyrific.Catapult.TaskProviders.DotNetCoreTest` as the built-in provider for Test Provider. This provider can be used to run test for the generated dotnet core application. It uses the `dotnet test` command that comes with the dotnet core sdk.

### Usage

This provider can only be used in Test task. You can use the name `Polyrific.Catapult.TaskProviders.DotNetCoreTest` when adding or updating a Test task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Test -t Test -prov Polyrific.Catapult.TaskProviders.DotNetCoreTest
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Test -prov Polyrific.Catapult.TaskProviders.DotNetCoreTest
```