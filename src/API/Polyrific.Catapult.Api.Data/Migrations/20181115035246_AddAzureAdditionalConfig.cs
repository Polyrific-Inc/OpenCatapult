using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddAzureAdditionalConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382263", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), false, true, "Connection String", "ConnectionString", 6, "string", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382263" });
        }
    }
}
