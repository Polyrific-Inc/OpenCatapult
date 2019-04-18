using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UpdateTo100beta4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "1.0.0-beta4" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a7", "1.0.0-beta4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "1.0.0-beta3" });

            migrationBuilder.UpdateData(
                table: "TaskProviders",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a7", "1.0.0-beta3" });
        }
    }
}
