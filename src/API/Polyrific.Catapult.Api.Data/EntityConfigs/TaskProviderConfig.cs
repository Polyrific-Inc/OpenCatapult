// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class TaskProviderConfig : BaseEntityConfig<TaskProvider>
    {
        public override void Configure(EntityTypeBuilder<TaskProvider> builder)
        {
            base.Configure(builder);

            builder.HasData(
                new TaskProvider
                {
                    Id = 1,
                    Name = "Polyrific.Catapult.TaskProviders.AspNetCoreMvc",
                    DisplayName = "AspNet Core Mvc Generator",
                    Description = "A generator task provider for generating an application with AspNet Core Mvc and Entity Framework Core backend",
                    ThumbnailUrl = "/assets/img/task-provider/aspnetcore.png",
                    Type = "GeneratorProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a1"
                },

                new TaskProvider
                {
                    Id = 2,
                    Name = "Polyrific.Catapult.TaskProviders.GitHub",
                    DisplayName = "GitHub Repository",
                    Description = "A repository task provider for cloning, pushing code, creating new branch and pull request into a GitHub repository",
                    ThumbnailUrl = "/assets/img/task-provider/github.png",
                    Type = "RepositoryProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    RequiredServicesString = "GitHub",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a2" },
                
                new TaskProvider
                {
                    Id = 3,
                    Name = "Polyrific.Catapult.TaskProviders.DotNetCore",
                    DisplayName = "DotNet Core Build",
                    Description = "A build task provider for building & publishing dotnet core application",
                    ThumbnailUrl = "/assets/img/task-provider/dotnetcore.png",
                    Type = "BuildProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a3"
                },

                new TaskProvider
                {
                    Id = 4,
                    Name = "Polyrific.Catapult.TaskProviders.DotNetCoreTest",
                    DisplayName = "Dotnet Core Test",
                    Description = "A test task provider for running a unit test project using \"dotnet test\" command",
                    ThumbnailUrl = "/assets/img/task-provider/dotnetcore.png",
                    Type = "TestProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a4"
                },

                new TaskProvider
                {
                    Id = 5,
                    Name = "Polyrific.Catapult.TaskProviders.EntityFrameworkCore",
                    DisplayName = "Entity Framework Core Database Migrator",
                    Description = "A Deploy database task provider for running the migration script with entity framework core to a designated database",
                    ThumbnailUrl = "/assets/img/task-provider/efcore.png",
                    Type = "DatabaseProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a5"
                },

                new TaskProvider
                {
                    Id = 6,
                    Name = "Polyrific.Catapult.TaskProviders.AzureAppService",
                    DisplayName = "Deploy To Azure App Service",
                    Description = "A deploy task provider for deploying an application to azure app service",
                    ThumbnailUrl = "/assets/img/task-provider/azureappservice.png",
                    Type = "HostingProvider",
                    Author = "Polyrific",
                    RequiredServicesString = "Azure",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a6"
                },

                new TaskProvider
                {
                    Id = 7,
                    Name = "Polyrific.Catapult.TaskProviders.GenericCommand",
                    DisplayName = "Generic Command",
                    Description = "A generic task provider for running any command in a preferred command line tools such as powershell or bash",
                    ThumbnailUrl = "/assets/img/task-provider/generic.png",
                    Type = "GenericTaskProvider",
                    Author = "Polyrific",
                    Version = "1.0.0-beta4",
                    Created = new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc),
                    ConcurrencyStamp = "976e0533-360a-4e46-8220-7c1cfdf0e0a7"
                }
            );
        }
    }
}
