using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenamePluginToProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginAdditionalConfigs");

            migrationBuilder.DropTable(
                name: "PluginTags");

            migrationBuilder.DropTable(
                name: "Plugins");

            migrationBuilder.CreateTable(
                name: "TaskProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    RequiredServicesString = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskProviderAdditionalConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TaskProviderId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false),
                    IsInputMasked = table.Column<bool>(nullable: true),
                    AllowedValues = table.Column<string>(nullable: true),
                    Hint = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviderAdditionalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskProviderAdditionalConfigs_TaskProviders_TaskProviderId",
                        column: x => x.TaskProviderId,
                        principalTable: "TaskProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskProviderTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TaskProviderId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviderTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskProviderTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskProviderTags_TaskProviders_TaskProviderId",
                        column: x => x.TaskProviderId,
                        principalTable: "TaskProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[,]
                {
                    { 1, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a1", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generator task provider for generating an application with AspNet Core Mvc and Entity Framework Core backend", "AspNet Core Mvc Generator", "Polyrific.Catapult.TaskProviders.AspNetCoreMvc", null, "/assets/img/task-provider/aspnetcore.png", "GeneratorProvider", null, "1.0.0-beta3" },
                    { 2, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a2", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A repository task provider for cloning, pushing code, creating new branch and pull request into a GitHub repository", "GitHub Repository", "Polyrific.Catapult.TaskProviders.GitHub", "GitHub", "/assets/img/task-provider/github.png", "RepositoryProvider", null, "1.0.0-beta3" },
                    { 3, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a3", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A build task provider for building & publishing dotnet core application", "DotNet Core Build", "Polyrific.Catapult.TaskProviders.DotNetCore", null, "/assets/img/task-provider/dotnetcore.png", "BuildProvider", null, "1.0.0-beta3" },
                    { 4, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a4", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A test task provider for running a unit test project using \"dotnet test\" command", "Dotnet Core Test", "Polyrific.Catapult.TaskProviders.DotNetCoreTest", null, "/assets/img/task-provider/dotnetcore.png", "TestProvider", null, "1.0.0-beta3" },
                    { 5, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a5", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A Deploy database task provider for running the migration script with entity framework core to a designated database", "Entity Framework Core Database Migrator", "Polyrific.Catapult.TaskProviders.EntityFrameworkCore", null, "/assets/img/task-provider/efcore.png", "DatabaseProvider", null, "1.0.0-beta3" },
                    { 6, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a6", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A deploy task provider for deploying an application to azure app service", "Deploy To Azure App Service", "Polyrific.Catapult.TaskProviders.AzureAppService", "Azure", "/assets/img/task-provider/azureappservice.png", "HostingProvider", null, "1.0.0-beta3" },
                    { 7, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a7", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generic task provider for running any command in a preferred command line tools such as powershell or bash", "Generic Command", "Polyrific.Catapult.TaskProviders.GenericCommand", null, "/assets/img/task-provider/generic.png", "GenericTaskProvider", null, "1.0.0-beta3" }
                });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[,]
                {
                    { 1, null, "c48cafcc-b3e9-4375-a2c2-f30404382258", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please enter the email address that you wish to be used as an administrator of the project", null, true, false, "Admin Email", "AdminEmail", 1, "string", null },
                    { 9, null, "c48cafcc-b3e9-4375-a2c2-f30404382260", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "App Service", "AppServiceName", 6, "string", null },
                    { 6, null, "c48cafcc-b3e9-4375-a2c2-f30404382262", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, false, true, true, "Connection String", "ConnectionString", 5, "string", null },
                    { 5, null, "c48cafcc-b3e9-4375-a2c2-f3040438225c", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Database Project Name", "DatabaseProjectName", 5, "string", null },
                    { 4, null, "c48cafcc-b3e9-4375-a2c2-f3040438225b", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Startup Project Name", "StartupProjectName", 5, "string", null },
                    { 10, null, "c48cafcc-b3e9-4375-a2c2-f30404382266", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Do you want to automatically reassign app service name when it is not available?", null, false, false, "Allow Automatic Rename?", "AllowAutomaticRename", 6, "boolean", null },
                    { 11, null, "c48cafcc-b3e9-4375-a2c2-f30404382261", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Deployment Slot", "DeploymentSlot", 6, "string", null },
                    { 12, null, "c48cafcc-b3e9-4375-a2c2-f30404382263", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please make sure to enter the connection string if the website needs to connect to the database", false, false, true, "Connection String", "ConnectionString", 6, "string", null },
                    { 13, null, "c48cafcc-b3e9-4375-a2c2-f30404382264", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default Region", "Region", 6, "string", null },
                    { 14, null, "c48cafcc-b3e9-4375-a2c2-f30404382265", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default App Service Plan", "AppServicePlan", 6, "string", null },
                    { 3, null, "c48cafcc-b3e9-4375-a2c2-f3040438225a", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Configuration", "Configuration", 3, "string", null },
                    { 8, null, "c48cafcc-b3e9-4375-a2c2-f3040438225f", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Resource Group", "ResourceGroupName", 6, "string", null },
                    { 2, null, "c48cafcc-b3e9-4375-a2c2-f30404382259", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Csproj Location", "CsprojLocation", 3, "string", null },
                    { 15, null, "c48cafcc-b3e9-4375-a2c2-f30404382267", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "The command tool to be used to run the command (e.g. Powershell). Defaults based on OS.", null, false, false, "Command Tool", "CommandTool", 7, "string", null },
                    { 16, null, "c48cafcc-b3e9-4375-a2c2-f30404382268", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Command Text", "CommandText", 7, "string", null },
                    { 17, null, "c48cafcc-b3e9-4375-a2c2-f30404382269", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "You can provide a script file (it is recommended to use this if the input contains multiple lines of commands)", null, false, false, "Command Script Path", "CommandScriptPath", 7, "file", null },
                    { 7, null, "c48cafcc-b3e9-4375-a2c2-f3040438225e", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Subscription Id", "SubscriptionId", 6, "string", null }
                });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[,]
                {
                    { 27, "21222bae-5e15-432c-ae4f-e671cb116d22", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 12, 6, null },
                    { 29, "21222bae-5e15-432c-ae4f-e671cb116d24", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 21, 6, null },
                    { 30, "21222bae-5e15-432c-ae4f-e671cb116d25", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 22, 6, null },
                    { 26, "21222bae-5e15-432c-ae4f-e671cb116d21", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 6, null },
                    { 25, "21222bae-5e15-432c-ae4f-e671cb116d20", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 6, null },
                    { 31, "21222bae-5e15-432c-ae4f-e671cb116d26", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 23, 6, null },
                    { 32, "21222bae-5e15-432c-ae4f-e671cb116d27", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 24, 6, null },
                    { 33, "21222bae-5e15-432c-ae4f-e671cb116d28", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 25, 7, null },
                    { 34, "21222bae-5e15-432c-ae4f-e671cb116d29", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 26, 7, null },
                    { 35, "21222bae-5e15-432c-ae4f-e671cb116d2a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 27, 7, null },
                    { 36, "21222bae-5e15-432c-ae4f-e671cb116d2b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 28, 7, null },
                    { 28, "21222bae-5e15-432c-ae4f-e671cb116d23", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 20, 6, null },
                    { 22, "21222bae-5e15-432c-ae4f-e671cb116d1c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 5, null },
                    { 23, "21222bae-5e15-432c-ae4f-e671cb116d1d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 5, null },
                    { 1, "21222bae-5e15-432c-ae4f-e671cb116d07", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 1, null },
                    { 2, "21222bae-5e15-432c-ae4f-e671cb116d08", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 1, null },
                    { 3, "21222bae-5e15-432c-ae4f-e671cb116d09", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 1, null },
                    { 4, "21222bae-5e15-432c-ae4f-e671cb116d0a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 9, 1, null },
                    { 5, "21222bae-5e15-432c-ae4f-e671cb116d0b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 10, 1, null },
                    { 6, "21222bae-5e15-432c-ae4f-e671cb116d0c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 11, 1, null },
                    { 7, "21222bae-5e15-432c-ae4f-e671cb116d0d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 12, 1, null },
                    { 8, "21222bae-5e15-432c-ae4f-e671cb116d0e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 13, 1, null },
                    { 9, "21222bae-5e15-432c-ae4f-e671cb116d0f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 14, 1, null },
                    { 10, "21222bae-5e15-432c-ae4f-e671cb116d10", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 2, null },
                    { 24, "21222bae-5e15-432c-ae4f-e671cb116d1f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 14, 5, null },
                    { 11, "21222bae-5e15-432c-ae4f-e671cb116d11", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 15, 2, null },
                    { 13, "21222bae-5e15-432c-ae4f-e671cb116d13", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 17, 2, null },
                    { 14, "21222bae-5e15-432c-ae4f-e671cb116d14", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 3, null },
                    { 15, "21222bae-5e15-432c-ae4f-e671cb116d15", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 3, null },
                    { 16, "21222bae-5e15-432c-ae4f-e671cb116d16", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 3, null },
                    { 17, "21222bae-5e15-432c-ae4f-e671cb116d17", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 4, null },
                    { 18, "21222bae-5e15-432c-ae4f-e671cb116d18", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 4, null },
                    { 19, "21222bae-5e15-432c-ae4f-e671cb116d19", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 18, 4, null },
                    { 20, "21222bae-5e15-432c-ae4f-e671cb116d1a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 19, 4, null },
                    { 21, "21222bae-5e15-432c-ae4f-e671cb116d1b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 5, null },
                    { 37, "21222bae-5e15-432c-ae4f-e671cb116d2c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 29, 7, null },
                    { 12, "21222bae-5e15-432c-ae4f-e671cb116d12", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 16, 2, null },
                    { 38, "21222bae-5e15-432c-ae4f-e671cb116d2d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 30, 7, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderAdditionalConfigs_TaskProviderId",
                table: "TaskProviderAdditionalConfigs",
                column: "TaskProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderTags_TagId",
                table: "TaskProviderTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderTags_TaskProviderId",
                table: "TaskProviderTags",
                column: "TaskProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskProviderAdditionalConfigs");

            migrationBuilder.DropTable(
                name: "TaskProviderTags");

            migrationBuilder.DropTable(
                name: "TaskProviders");

            migrationBuilder.CreateTable(
                name: "Plugins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Author = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RequiredServicesString = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    Version = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plugins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluginAdditionalConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllowedValues = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Hint = table.Column<string>(nullable: true),
                    IsInputMasked = table.Column<bool>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PluginId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginAdditionalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PluginAdditionalConfigs_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PluginTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    PluginId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PluginTags_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PluginTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Plugins",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[,]
                {
                    { 1, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a1", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generator task provider for generating an application with AspNet Core Mvc and Entity Framework Core backend", "AspNet Core Mvc Generator", "Polyrific.Catapult.TaskProviders.AspNetCoreMvc", null, "/assets/img/task-provider/aspnetcore.png", "GeneratorProvider", null, "1.0.0-beta3" },
                    { 2, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a2", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A repository task provider for cloning, pushing code, creating new branch and pull request into a GitHub repository", "GitHub Repository", "Polyrific.Catapult.TaskProviders.GitHub", "GitHub", "/assets/img/task-provider/github.png", "RepositoryProvider", null, "1.0.0-beta3" },
                    { 3, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a3", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A build task provider for building & publishing dotnet core application", "DotNet Core Build", "Polyrific.Catapult.TaskProviders.DotNetCore", null, "/assets/img/task-provider/dotnetcore.png", "BuildProvider", null, "1.0.0-beta3" },
                    { 4, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a4", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A test task provider for running a unit test project using \"dotnet test\" command", "Dotnet Core Test", "Polyrific.Catapult.TaskProviders.DotNetCoreTest", null, "/assets/img/task-provider/dotnetcore.png", "TestProvider", null, "1.0.0-beta3" },
                    { 5, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a5", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A Deploy database task provider for running the migration script with entity framework core to a designated database", "Entity Framework Core Database Migrator", "Polyrific.Catapult.TaskProviders.EntityFrameworkCore", null, "/assets/img/task-provider/efcore.png", "DatabaseProvider", null, "1.0.0-beta3" },
                    { 6, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a6", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A deploy task provider for deploying an application to azure app service", "Deploy To Azure App Service", "Polyrific.Catapult.TaskProviders.AzureAppService", "Azure", "/assets/img/task-provider/azureappservice.png", "HostingProvider", null, "1.0.0-beta3" },
                    { 7, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a7", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generic task provider for running any command in a preferred command line tools such as powershell or bash", "Generic Command", "Polyrific.Catapult.TaskProviders.GenericCommand", null, "/assets/img/task-provider/generic.png", "GenericTaskProvider", null, "1.0.0-beta3" }
                });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[,]
                {
                    { 1, null, "c48cafcc-b3e9-4375-a2c2-f30404382258", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please enter the email address that you wish to be used as an administrator of the project", null, true, false, "Admin Email", "AdminEmail", 1, "string", null },
                    { 9, null, "c48cafcc-b3e9-4375-a2c2-f30404382260", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "App Service", "AppServiceName", 6, "string", null },
                    { 6, null, "c48cafcc-b3e9-4375-a2c2-f30404382262", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, false, true, true, "Connection String", "ConnectionString", 5, "string", null },
                    { 5, null, "c48cafcc-b3e9-4375-a2c2-f3040438225c", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Database Project Name", "DatabaseProjectName", 5, "string", null },
                    { 4, null, "c48cafcc-b3e9-4375-a2c2-f3040438225b", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Startup Project Name", "StartupProjectName", 5, "string", null },
                    { 10, null, "c48cafcc-b3e9-4375-a2c2-f30404382266", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Do you want to automatically reassign app service name when it is not available?", null, false, false, "Allow Automatic Rename?", "AllowAutomaticRename", 6, "boolean", null },
                    { 11, null, "c48cafcc-b3e9-4375-a2c2-f30404382261", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Deployment Slot", "DeploymentSlot", 6, "string", null },
                    { 12, null, "c48cafcc-b3e9-4375-a2c2-f30404382263", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please make sure to enter the connection string if the website needs to connect to the database", false, false, true, "Connection String", "ConnectionString", 6, "string", null },
                    { 13, null, "c48cafcc-b3e9-4375-a2c2-f30404382264", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default Region", "Region", 6, "string", null },
                    { 14, null, "c48cafcc-b3e9-4375-a2c2-f30404382265", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default App Service Plan", "AppServicePlan", 6, "string", null },
                    { 3, null, "c48cafcc-b3e9-4375-a2c2-f3040438225a", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Configuration", "Configuration", 3, "string", null },
                    { 8, null, "c48cafcc-b3e9-4375-a2c2-f3040438225f", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Resource Group", "ResourceGroupName", 6, "string", null },
                    { 2, null, "c48cafcc-b3e9-4375-a2c2-f30404382259", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Csproj Location", "CsprojLocation", 3, "string", null },
                    { 15, null, "c48cafcc-b3e9-4375-a2c2-f30404382267", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "The command tool to be used to run the command (e.g. Powershell). Defaults based on OS.", null, false, false, "Command Tool", "CommandTool", 7, "string", null },
                    { 16, null, "c48cafcc-b3e9-4375-a2c2-f30404382268", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Command Text", "CommandText", 7, "string", null },
                    { 17, null, "c48cafcc-b3e9-4375-a2c2-f30404382269", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "You can provide a script file (it is recommended to use this if the input contains multiple lines of commands)", null, false, false, "Command Script Path", "CommandScriptPath", 7, "file", null },
                    { 7, null, "c48cafcc-b3e9-4375-a2c2-f3040438225e", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Subscription Id", "SubscriptionId", 6, "string", null }
                });

            migrationBuilder.InsertData(
                table: "PluginTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "PluginId", "TagId", "Updated" },
                values: new object[,]
                {
                    { 27, "21222bae-5e15-432c-ae4f-e671cb116d22", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 12, null },
                    { 29, "21222bae-5e15-432c-ae4f-e671cb116d24", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 21, null },
                    { 30, "21222bae-5e15-432c-ae4f-e671cb116d25", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 22, null },
                    { 26, "21222bae-5e15-432c-ae4f-e671cb116d21", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 8, null },
                    { 25, "21222bae-5e15-432c-ae4f-e671cb116d20", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 6, null },
                    { 31, "21222bae-5e15-432c-ae4f-e671cb116d26", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 23, null },
                    { 32, "21222bae-5e15-432c-ae4f-e671cb116d27", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 24, null },
                    { 33, "21222bae-5e15-432c-ae4f-e671cb116d28", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 25, null },
                    { 34, "21222bae-5e15-432c-ae4f-e671cb116d29", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 26, null },
                    { 35, "21222bae-5e15-432c-ae4f-e671cb116d2a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 27, null },
                    { 36, "21222bae-5e15-432c-ae4f-e671cb116d2b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 28, null },
                    { 28, "21222bae-5e15-432c-ae4f-e671cb116d23", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 20, null },
                    { 22, "21222bae-5e15-432c-ae4f-e671cb116d1c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 7, null },
                    { 23, "21222bae-5e15-432c-ae4f-e671cb116d1d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 8, null },
                    { 1, "21222bae-5e15-432c-ae4f-e671cb116d07", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 1, null },
                    { 2, "21222bae-5e15-432c-ae4f-e671cb116d08", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 7, null },
                    { 3, "21222bae-5e15-432c-ae4f-e671cb116d09", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 8, null },
                    { 4, "21222bae-5e15-432c-ae4f-e671cb116d0a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 9, null },
                    { 5, "21222bae-5e15-432c-ae4f-e671cb116d0b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 10, null },
                    { 6, "21222bae-5e15-432c-ae4f-e671cb116d0c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 11, null },
                    { 7, "21222bae-5e15-432c-ae4f-e671cb116d0d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 12, null },
                    { 8, "21222bae-5e15-432c-ae4f-e671cb116d0e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 13, null },
                    { 9, "21222bae-5e15-432c-ae4f-e671cb116d0f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 14, null },
                    { 10, "21222bae-5e15-432c-ae4f-e671cb116d10", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 2, null },
                    { 24, "21222bae-5e15-432c-ae4f-e671cb116d1f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 14, null },
                    { 11, "21222bae-5e15-432c-ae4f-e671cb116d11", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 15, null },
                    { 13, "21222bae-5e15-432c-ae4f-e671cb116d13", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 17, null },
                    { 14, "21222bae-5e15-432c-ae4f-e671cb116d14", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 3, null },
                    { 15, "21222bae-5e15-432c-ae4f-e671cb116d15", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 7, null },
                    { 16, "21222bae-5e15-432c-ae4f-e671cb116d16", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 8, null },
                    { 17, "21222bae-5e15-432c-ae4f-e671cb116d17", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 4, null },
                    { 18, "21222bae-5e15-432c-ae4f-e671cb116d18", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 7, null },
                    { 19, "21222bae-5e15-432c-ae4f-e671cb116d19", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 18, null },
                    { 20, "21222bae-5e15-432c-ae4f-e671cb116d1a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 19, null },
                    { 21, "21222bae-5e15-432c-ae4f-e671cb116d1b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 5, null },
                    { 37, "21222bae-5e15-432c-ae4f-e671cb116d2c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 29, null },
                    { 12, "21222bae-5e15-432c-ae4f-e671cb116d12", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 16, null },
                    { 38, "21222bae-5e15-432c-ae4f-e671cb116d2d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 30, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginAdditionalConfigs_PluginId",
                table: "PluginAdditionalConfigs",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_PluginTags_PluginId",
                table: "PluginTags",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_PluginTags_TagId",
                table: "PluginTags",
                column: "TagId");
        }
    }
}
