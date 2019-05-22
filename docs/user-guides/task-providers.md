# Manage task providers

Aside from the built-in catapult provider in each job task definition, you can install another task provider which you can [write yourself](../dev-guides/create-task-provider.md), or download from [marketplace](../dev-guides/create-task-provider-4.md#provider-marketplace).

## Install Task Provider

To install a task provider, use the `provider register` command, and specify the path to `taskprovider.yml` file:
```sh
dotnet occli.dll provider register --file d:\MyTaskProvider\taskprovider.yml
```

## Remove Task Provider

To uninstall the task provider, use the name of the task provider as the parameter:
```sh
dotnet occli.dll provider remove --name MyTaskProvider 
```
