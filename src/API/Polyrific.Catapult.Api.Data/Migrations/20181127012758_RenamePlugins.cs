using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenamePlugins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "Polyrific.Catapult.Plugins.AspNetCoreMvc" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "Polyrific.Catapult.Plugins.GitHub" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "Polyrific.Catapult.Plugins.DotNetCore" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "Polyrific.Catapult.Plugins.DotNetCoreTest" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "Polyrific.Catapult.Plugins.EntityFrameworkCore" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "Polyrific.Catapult.Plugins.AzureAppService" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "AspNetCoreMvc" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "GitHubRepositoryProvider" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "DotNetCoreBuildProvider" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "DotNetCoreTest" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "EntityFrameworkCore" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "AzureAppService" });
        }
    }
}
