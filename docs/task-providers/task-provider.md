# Task Provider

Task Provider is `OpenCatapult` extension which provides specific implementation of a job task. It holds an important role because the `OpenCatapult` itself is designed to be "platform-agnostic system", which means it should not make assumption on how a task should work. `OpenCatapult` just provides the skeleton of the tasks so they can be executed correctly in the defined sequence. That's why you are required to define a provider when defining a job task.

There are 7 types of Task Provider. `OpenCatapult` provides built-in Task Providers for most of them:

- [Generator Provider](generator-provider.md), with `Polyrific.Catapult.Plugins.AspNetCoreMvc` as the default provider
- [Build Provider](build-provider.md), with `Polyrific.Catapult.Plugins.DotNetCore` as the default provider
- [Repository Provider](repository-provider.md), with `Polyrific.Catapult.Plugins.GitHub` as the default provider
- [Hosting Provider](hosting-provider.md), with `Polyrific.Catapult.Plugins.AzureAppService` as the default provider
- [Database Provider](database-provider.md), with `Polyrific.Catapult.Plugins.EntityFrameworkCore` as the default provider
- [Test Provider](test-provider.md), with `Polyrific.Catapult.Plugins.DotNetCoreTest` as the default provider
- [Storage Provider](storage-provider.md)