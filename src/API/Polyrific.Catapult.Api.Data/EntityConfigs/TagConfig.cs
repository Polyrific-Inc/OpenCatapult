// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Polyrific.Catapult.Api.Core.Entities;

namespace Polyrific.Catapult.Api.Data.EntityConfigs
{
    public class TagConfig : BaseEntityConfig<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(t => t.Name).IsRequired();
            builder.HasIndex(t => t.Name).IsUnique();

            builder.HasData(
                new Tag { Id = 1, Name = "Code Generator", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc3e" },
                new Tag { Id = 2, Name = "Repository", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc3f" },
                new Tag { Id = 3, Name = "Build", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc40" },
                new Tag { Id = 4, Name = "Test", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc41" },
                new Tag { Id = 5, Name = "Database Deploy", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc42" },
                new Tag { Id = 6, Name = "Deploy", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc43" },
                new Tag { Id = 7, Name = "DotNet Core", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc44" },
                new Tag { Id = 8, Name = "Microsoft", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc45" },
                new Tag { Id = 9, Name = "AspNet Core", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc46" },
                new Tag { Id = 10, Name = "AspNet Core Mvc", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc47" },
                new Tag { Id = 11, Name = "MVC", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc48" },
                new Tag { Id = 12, Name = "Web", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc49" },
                new Tag { Id = 13, Name = "CRUD", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc4a" },
                new Tag { Id = 14, Name = "Entity Framework Core", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc4b" },
                new Tag { Id = 15, Name = "GitHub", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc4c" },
                new Tag { Id = 16, Name = "Source Control", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc4e" },
                new Tag { Id = 17, Name = "Git", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc4f" },
                new Tag { Id = 18, Name = "Unit Test", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc50" },
                new Tag { Id = 19, Name = "xUnit", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc51" },
                new Tag { Id = 20, Name = "Azure", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc52" },
                new Tag { Id = 21, Name = "Azure App Service", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc53" },
                new Tag { Id = 22, Name = "Hosting", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc54" },
                new Tag { Id = 23, Name = "Cloud", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc55" },
                new Tag { Id = 24, Name = "PaaS", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc56" },
                new Tag { Id = 25, Name = "Generic", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc357" },
                new Tag { Id = 26, Name = "Command", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc58" },
                new Tag { Id = 27, Name = "Command Line", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc59" },
                new Tag { Id = 28, Name = "CLI", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc5a" },
                new Tag { Id = 29, Name = "Tool", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc5b" },
                new Tag { Id = 30, Name = "Custom", Created = new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), ConcurrencyStamp = "7c29af83-c493-4f23-a600-e5f9d1d2bc5c" }
                );
        }
    }
}
