using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class CompleteVersionInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "CatapultEngineProfile",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "1.0.0-beta1" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "1.0.0-beta1" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "1.0.0-beta1" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "1.0.0-beta1" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "1.0.0-beta1" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "1.0.0-beta1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "CatapultEngineProfile");

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "1.0" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "1.0" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "1.0" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "1.0" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "1.0" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Version" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "1.0" });
        }
    }
}
