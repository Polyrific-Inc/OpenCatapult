using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenameEFCoreAndGitHub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "Polyrific.Catapult.TaskProviders.GitHub" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "Polyrific.Catapult.TaskProviders.EntityFrameworkCore" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "Polyrific.Catapult.Plugins.GitHub" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "Polyrific.Catapult.Plugins.EntityFrameworkCore" });
        }
    }
}
