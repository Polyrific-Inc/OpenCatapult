using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AllowHostingRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ConcurrencyStamp", "IsRequired" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382260", false });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ConcurrencyStamp", "Hint", "Label", "Name", "Type" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382266", "Do you want to automatically reassign app service name when it is not available?", "Allow Automatic Rename?", "AllowAutomaticRename", "boolean" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ConcurrencyStamp", "Hint", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382261", null, null, false, "Deployment Slot", "DeploymentSlot" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ConcurrencyStamp", "Hint", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382263", "Please make sure to enter the connection string if the website needs to connect to the database", false, true, "Connection String", "ConnectionString" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382264", "Default Region", "Region" });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 14, null, "c48cafcc-b3e9-4375-a2c2-f30404382265", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default App Service Plan", "AppServicePlan", 6, "string", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 14, "c48cafcc-b3e9-4375-a2c2-f30404382265" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ConcurrencyStamp", "IsRequired" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382260", true });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ConcurrencyStamp", "Hint", "Label", "Name", "Type" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382261", null, "Deployment Slot", "DeploymentSlot", "string" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ConcurrencyStamp", "Hint", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382263", "Please make sure to enter the connection string if the website needs to connect to the database", false, true, "Connection String", "ConnectionString" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ConcurrencyStamp", "Hint", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382264", null, null, false, "Default Region", "Region" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382265", "Default App Service Plan", "AppServicePlan" });
        }
    }
}
