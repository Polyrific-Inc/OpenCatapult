using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations.CatapultSqliteDb
{
    public partial class DefaultJobDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "JobDefinitions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "JobDefinitions");
        }
    }
}
