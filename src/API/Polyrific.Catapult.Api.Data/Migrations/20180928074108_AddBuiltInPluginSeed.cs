using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddBuiltInPluginSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plugins",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Name", "RequiredServicesString", "Type", "Updated", "Version" },
                values: new object[,]
                {
                    { 1, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a1", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "AspNetCoreMvc", null, "GeneratorProvider", null, "1.0" },
                    { 2, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a2", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "GitHubRepositoryProvider", "GitHub", "RepositoryProvider", null, "1.0" },
                    { 3, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a3", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "DotNetCoreBuildProvider", null, "BuildProvider", null, "1.0" },
                    { 4, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a4", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "DotNetCoreTest", null, "TestProvider", null, "1.0" },
                    { 5, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a5", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "EntityFrameworkCore", null, "DatabaseProvider", null, "1.0" },
                    { 6, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a6", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "AzureAppService", "AzureAppService", "HostingProvider", null, "1.0" }
                });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[,]
                {
                    { 1, "c48cafcc-b3e9-4375-a2c2-f30404382258", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), true, true, "Connection String", "ConnectionString", 1, "string", null },
                    { 2, "c48cafcc-b3e9-4375-a2c2-f30404382259", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), true, false, "Subscription Id", "SubscriptionId", 6, "string", null },
                    { 3, "c48cafcc-b3e9-4375-a2c2-f3040438225a", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), true, false, "Resource Group", "ResourceGroupName", 6, "string", null },
                    { 4, "c48cafcc-b3e9-4375-a2c2-f3040438225b", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), true, false, "App Service", "AppServiceName", 6, "string", null },
                    { 5, "c48cafcc-b3e9-4375-a2c2-f3040438225c", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), false, false, "Deployment Slot", "DeploymentSlot", 6, "string", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 1, "c48cafcc-b3e9-4375-a2c2-f30404382258" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 2, "c48cafcc-b3e9-4375-a2c2-f30404382259" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 3, "c48cafcc-b3e9-4375-a2c2-f3040438225a" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 4, "c48cafcc-b3e9-4375-a2c2-f3040438225b" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 5, "c48cafcc-b3e9-4375-a2c2-f3040438225c" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 2, "976e0533-360a-4e46-8220-7c1cfdf0e0a2" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 3, "976e0533-360a-4e46-8220-7c1cfdf0e0a3" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 4, "976e0533-360a-4e46-8220-7c1cfdf0e0a4" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 5, "976e0533-360a-4e46-8220-7c1cfdf0e0a5" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 1, "976e0533-360a-4e46-8220-7c1cfdf0e0a1" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 6, "976e0533-360a-4e46-8220-7c1cfdf0e0a6" });
        }
    }
}
