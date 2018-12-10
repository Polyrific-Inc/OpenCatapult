using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AdditionalConfigInputMask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInputMasked",
                table: "PluginAdditionalConfigs",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "IsInputMasked" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382262", false });

            migrationBuilder.UpdateData(
                table: "PluginAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ConcurrencyStamp", "IsInputMasked" },
                values: new object[] { "c48cafcc-b3e9-4375-a2c2-f30404382263", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInputMasked",
                table: "PluginAdditionalConfigs");
        }
    }
}
