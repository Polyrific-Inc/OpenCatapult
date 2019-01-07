using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RemoveConfigurationDeployDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 12, "c48cafcc-b3e9-4375-a2c2-f30404382263" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "IsRequired", "Label", "Name", "PluginId" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f3040438225e", true, "Subscription Id", "SubscriptionId", 6 });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f3040438225f", "Resource Group", "ResourceGroupName" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382260", "App Service", "AppServiceName" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ConcurrencyStamp", "IsRequired", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382261", false, "Deployment Slot", "DeploymentSlot" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ConcurrencyStamp", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382263", false, true, "Connection String", "ConnectionString" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "IsRequired", "Label", "Name", "PluginId" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f3040438225d", false, "Configuration", "Configuration", 5 });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f3040438225e", "Subscription Id", "SubscriptionId" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ConcurrencyStamp", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f3040438225f", "Resource Group", "ResourceGroupName" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ConcurrencyStamp", "IsRequired", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382260", true, "App Service", "AppServiceName" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ConcurrencyStamp", "IsInputMasked", "IsSecret", "Label", "Name" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382261", null, false, "Deployment Slot", "DeploymentSlot" });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 12, null, "c48cafcc-b3e9-4375-a2c2-f30404382263", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), false, false, true, "Connection String", "ConnectionString", 6, "string", null });
        }
    }
}
