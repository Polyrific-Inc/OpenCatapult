using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenameDotNetCoreAndTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "Polyrific.Catapult.TaskProviders.DotNetCore" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "Polyrific.Catapult.TaskProviders.DotNetCoreTest" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
