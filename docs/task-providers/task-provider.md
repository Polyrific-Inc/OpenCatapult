# Task Provider

A "Task Provider" is an `OpenCatapult` extension which provides specific implementation of a job task. It holds an important role because  `OpenCatapult` itself is designed to be a platform-agnostic system which means it should not make any assumptions about how a task should work. `OpenCatapult` provides the mechanism through which task providers execute, correctly and in the defined sequence. You can think of `OpenCatapult` as a cassette player and the task providers as the cassettes: 

![alt text](https://opencatapult.net/img/CatapultCassette.jpg "OpenCatapult & Task Providers")

The task provider (cassette) contains the music that will be generated whereas the cassette player (`OpenCatapult`) simply plays the cassette tape. 

If you went through the quick start you may have noticed that you had to define a task provider when creating your first project. That's because without a "tape", the "cassette player" would have no music to play.

At the moment, there are 8 types of Task Provider. `OpenCatapult` provides built-in Task Providers for most of them:

- [Generator Provider](generator-provider.md)

  A "Generator Provider" defines **what** you are asking `OpenCatapult` to create. For example, you may consume or create a provider that builds a .NET MVC based intranet, or a PHP based eCommerce site. A task provider can even be created for an Amazon Alexa voice app; `OpenCatapult` doesn't care what is music is on the cassette tape, it just knows how to play whatever you have selected. 
  
  The default Generator Provider is `Polyrific.Catapult.TaskProviders.AspNetCoreMvc` which produces an ASP.NET Core MVC application.

- [Build Provider](build-provider.md)

  Build Providers answer the question of **how** your code will be built and deployed. `OpenCatapult` allows you to define a CI/CD pipeline that works your way by creating a build provider that defines every actionable step in your build & release flow.

  The default Build Provider is `Polyrific.Catapult.TaskProviders.DotNetCore` which builds a Dotnet Core application into a ready to deploy artifact.

- [Repository Provider](repository-provider.md)

  The Repository Provider dictates **where** your source code will live. You can create or consume task providers for almost any kind of version control service so that your code is checked in to a location of your choosing. 

  The default Repository Provider is `Polyrific.Catapult.TaskProviders.GitHub` which stores the source code to GitHub repository.

- [Hosting Provider](hosting-provider.md)

  The Hosting Provider lets you declare **where** your app should be deployed. You can create or consume a hosting provider that will deploy your apps to Azure, AWS, Rackspace, on-premise, or any other place of your choosing.

  The default Hosting Provider is `Polyrific.Catapult.TaskProviders.AzureAppService` which provides the implementation of application deployment to Azure App Service.

- [Database Provider](database-provider.md)
  
  The Database Provider handles how the changes on the defined data models are reflected to the application database. You can create or consume a Database Provider for specific database services (e.g. SQL Server, MySQL, PostgreSQL, etc) with optional ORM (e.g. Entity Framework, NHibernate, ActiveRecord, etc).

  The default Database Provider is `Polyrific.Catapult.TaskProviders.EntityFrameworkCore` which provides the implementation of Entity Framework Core to work with SQL Server database.

- [Test Provider](test-provider.md)
  
  The Test Provider lets you define various tests for your application. You can create or consume a Test Provider for specific test runners, e.g. XUnit, NUnit, Karma, etc.

  The default Test Provider is `Polyrific.Catapult.TaskProviders.DotNetCoreTest` which utilizes Dotner Core Test tool to execute test scenarios for the defined test runner.

- [Storage Provider](storage-provider.md)

  The Storage Provider facilitates the need to upload or download an artifact in the case where the Build Server and Deployment Server are located in different machines.

- [Generic Task Provider](generic-task-provider.md)

  The Generic Task Provider helps you to create a simple action without having to create a specific task provider for it. It can be any actions like copying files, replacing file content, checking installed tools, etc.

  The default Generic Task Provider is `Polyrific.Catapult.TaskProviders.GenericCommand` which allows user to enter some commands to be run in a command line tool. In most of the time it will be the only Generic Task Provider you ever need.