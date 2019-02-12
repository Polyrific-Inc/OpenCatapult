# Task Provider

A "Task Provider" is an `OpenCatapult` extension which provides specific implementation of a job task. It holds an important role because  `OpenCatapult` itself is designed to be a platform-agnostic system which means it should not make any assumptions about how a task should work. `OpenCatapult` provides the mechanism through which task providers execute, correctly and in the defined sequence. You can think of `OpenCatapult` as a cassette player and the task providers as the cassettes: 

![alt text](https://opencatapult.net/img/CatapultCassette.jpg "OpenCatapult & Task Providers")

The task provider (cassette) contains the music that will be generated whereas the cassette player (`OpenCatapult`) simply plays the cassette tape. 

If you went through the quick start you may have noticed that you had to define a task provider when creating your first project. That's because without a "tape", the "cassette player" would have no music to play.

There are 7 types of Task Provider. `OpenCatapult` provides built-in Task Providers for most of them:

- [Generator Provider](generator-provider.md)

  A "Generator Provider" defines **what** you are asking `OpenCatapult` to create. For example, you may consume or create a provider that builds a .NET MVC based intranet, or a PHP based eCommerce site. A task provider can even be created for an Amazon Alexa voice app; `OpenCatapult` doesn't care what is music is on the cassette tape, it just knows how to play whatever you have selected. 
  
  The default generator provider is `Polyrific.Catapult.Plugins.AspNetCoreMvc`

- [Build Provider](build-provider.md)

  Build Providers answer the question of **how** your code will be built and deployed. `OpenCatapult` allows you to define a CI/CD pipeline that works your way by creating a build provider that defines every actionable step in your build & release flow.

  The default build provider is `Polyrific.Catapult.Plugins.DotNetCore`

- [Repository Provider](repository-provider.md)

  The Repository Provider dictates **where** your source code will live. You can create or consume task providers for almost any kind of version control service so that your code is checked in to a location of your choosing. 

  The default repository provider is `Polyrific.Catapult.Plugins.GitHub`

- [Hosting Provider](hosting-provider.md)

  The Hosting Provider lets you declare **where** your app should be deployed. You can create or consume a hosting provider that will deploy your apps to Azure, AWS, Rackspace, on-premise, or any other place of your choosing.

  The default hosting provider is `Polyrific.Catapult.Plugins.AzureAppService`.

- [Database Provider](database-provider.md), with `Polyrific.Catapult.Plugins.EntityFrameworkCore` as the default provider
- [Test Provider](test-provider.md), with `Polyrific.Catapult.Plugins.DotNetCoreTest` as the default provider
- [Storage Provider](storage-provider.md)
