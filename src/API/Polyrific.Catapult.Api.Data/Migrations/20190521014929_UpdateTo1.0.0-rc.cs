using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UpdateTo100rc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 2,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 3,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 4,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 5,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 6,
                column: "Version",
                value: "1.0.0-rc");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 7,
                column: "Version",
                value: "1.0.0-rc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 2,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 3,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 4,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 5,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 6,
                column: "Version",
                value: "1.0.0-beta4");

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 7,
                column: "Version",
                value: "1.0.0-beta4");
        }
    }
}
