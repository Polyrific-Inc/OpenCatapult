using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class GenericExternalService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 3, "2425fe0d-4e3e-4549-a9a7-60056097ce98", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "GenericService", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExternalServiceTypes",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 3, "2425fe0d-4e3e-4549-a9a7-60056097ce98" });
        }
    }
}
