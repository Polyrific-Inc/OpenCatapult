using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AdditionalConfigHint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "PluginAdditionalConfigs",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Hint" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382258", "Please enter the email address that you wish to be used as an administrator of the project" });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ConcurrencyStamp", "Hint" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382263", "Please make sure to enter the connection string if the website needs to connect to the database" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hint",
                table: "PluginAdditionalConfigs");
        }
    }
}
