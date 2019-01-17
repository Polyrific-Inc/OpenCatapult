using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RenameAzureService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalServiceTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "2425fe0d-4e3e-4549-a9a7-60056097ce97", "Azure" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RequiredServicesString" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "Azure" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalServiceTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Name" },
                values: new object[] { "2425fe0d-4e3e-4549-a9a7-60056097ce97", "AzureAppService" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "RequiredServicesString" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "AzureAppService" });
        }
    }
}
