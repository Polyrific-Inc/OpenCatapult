using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenameAspNetCoreMvcAzureAppService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "Polyrific.Catapult.TaskProviders.AspNetCoreMvc" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "Polyrific.Catapult.TaskProviders.AzureAppService" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "Polyrific.Catapult.Plugins.AzureAppService" });
        }
    }
}
