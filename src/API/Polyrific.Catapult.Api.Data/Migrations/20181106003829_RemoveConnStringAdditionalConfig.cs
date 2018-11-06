using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RemoveConnStringAdditionalConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382300" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382258", false, "Admin Email", "AdminEmail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382258", true, "Connection String", "ConnectionString" });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382300", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), true, false, "Admin Email", "AdminEmail", 1, "string", null });
        }
    }
}
