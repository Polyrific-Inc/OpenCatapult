# Generic Task Provider

Generic Task Provider can be used in [Custom Task](../user-guides/job-definitions.md#custom-task). It helps you to create a simple action without having to create a specific task provider for it. It can be any actions like copying files, replacing file content, checking installed tools, etc.

## Polyrific.Catapult.TaskProviders.GenericCommand

`OpenCatapult` provides `Polyrific.Catapult.TaskProviders.GenericCommand` as the built-in provider for Generic Task Provider. It allows user to enter some commands to be run in a command line tool. In most of the time it will be the only Generic Task Provider the users ever need.

### Usage

This provider can only be used in Custom Task. You can use the name `Polyrific.Catapult.TaskProviders.GenericCommand` when adding or updating a Custom Task:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n CopyConfigFile -t CustomTask -prov Polyrific.Catapult.TaskProviders.GenericCommand
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n CopyConfigFile -prov Polyrific.Catapult.TaskProviders.GenericCommand
```