using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AzureAppServiceSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 2, "2425fe0d-4e3e-4549-a9a7-60056097ce97", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "AzureAppService", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Updated" },
                values: new object[] { 6, null, "416fcf67-35cf-4ea3-b534-dade4a81da89", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Application Id", 2, true, false, "ApplicationId", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Updated" },
                values: new object[] { 7, null, "416fcf67-35cf-4ea3-b534-dade4a81da8a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Application Key", 2, true, true, "ApplicationKey", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Updated" },
                values: new object[] { 8, null, "416fcf67-35cf-4ea3-b534-dade4a81da8b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Tenant Id", 2, true, false, "TenantId", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExternalServiceProperties",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 6, "416fcf67-35cf-4ea3-b534-dade4a81da89" });

            migrationBuilder.DeleteData(
                table: "ExternalServiceProperties",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 7, "416fcf67-35cf-4ea3-b534-dade4a81da8a" });

            migrationBuilder.DeleteData(
                table: "ExternalServiceProperties",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 8, "416fcf67-35cf-4ea3-b534-dade4a81da8b" });

            migrationBuilder.DeleteData(
                table: "ExternalServiceTypes",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 2, "2425fe0d-4e3e-4549-a9a7-60056097ce97" });
        }
    }
}
