# Repository Provider

Repository providers can be used by [Clone task](../user-guides/job-definitions.md#clone), [Push task](../user-guides/job-definitions.md#push), and [Merge task](../user-guides/job-definitions.md#merge). The main role of this provider is to provide a specific implementation to work with remote code repository service.

## Polyrific.Catapult.TaskProviders.GitHub

`OpenCatapult` provides `Polyrific.Catapult.TaskProviders.GitHub` as the built-in provider for Repository Provider. This provider can be used to clone, push, create and merge pull request in a GitHub repository.

### Usage

This provider can be used in Clone, Push, and Merge task. You can use the name `Polyrific.Catapult.TaskProviders.GitHub` when adding or updating the tasks:

```sh
dotnet occli.dll task add -p SampleProject -j Default -n Clone -t Clone -prov Polyrific.Catapult.TaskProviders.GitHub
```

```sh
dotnet occli.dll task update -p SampleProject -j Default -n Clone -prov Polyrific.Catapult.TaskProviders.GitHub
```

### Required services

This provider requires the `GitHub` external service to get the connection details. Please refer to the [External Service guideline](../user-guides/external-services.md#GitHub) for more info.