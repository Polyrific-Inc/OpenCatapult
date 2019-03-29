using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class GenericTaskProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plugins",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Name", "RequiredServicesString", "Type", "Updated", "Version" },
                values: new object[] { 7, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a7", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Polyrific.Catapult.TaskProviders.GenericCommand", null, "GenericTaskProvider", null, "1.0.0-beta3" });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 15, null, "c48cafcc-b3e9-4375-a2c2-f30404382267", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "The command tool to be used to run the command (e.g. Powershell). Defaults based on OS.", null, false, false, "Command Tool", "CommandTool", 7, "string", null });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 16, null, "c48cafcc-b3e9-4375-a2c2-f30404382268", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Command Text", "CommandText", 7, "string", null });

            migrationBuilder.InsertData(
                table: "PluginAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "PluginId", "Type", "Updated" },
                values: new object[] { 17, null, "c48cafcc-b3e9-4375-a2c2-f30404382269", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "You can provide a script file (it is recommended to use this if the input contains multiple lines of commands)", null, false, false, "Command Script Path", "CommandScriptPath", 7, "file", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 15, "c48cafcc-b3e9-4375-a2c2-f30404382267" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 16, "c48cafcc-b3e9-4375-a2c2-f30404382268" });

            migrationBuilder.DeleteData(
                table: "PluginAdditionalConfigs",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 17, "c48cafcc-b3e9-4375-a2c2-f30404382269" });

            migrationBuilder.DeleteData(
                table: "Plugins",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 7, "976e0533-360a-4e46-8220-7c1cfdf0e0a7" });
        }
    }
}
