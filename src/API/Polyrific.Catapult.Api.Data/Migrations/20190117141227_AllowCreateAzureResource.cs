using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AllowCreateAzureResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 12, null, "c48cafcc-b3e9-4375-a2c2-f30404382264", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, false, false, "Default Region", "Region", 6, "string", null });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 13, null, "c48cafcc-b3e9-4375-a2c2-f30404382265", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, false, false, "Default App Service Plan", "AppServicePlan", 6, "string", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382264" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 13, "c48cafcc-b3e9-4375-a2c2-f30404382265" });
        }
    }
}
