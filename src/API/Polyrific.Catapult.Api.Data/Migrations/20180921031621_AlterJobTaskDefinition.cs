using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AlterJobTaskDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContinueWhenError",
                table: "JobTaskDefinitions");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "JobTaskDefinitions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                table: "JobTaskDefinitions");

            migrationBuilder.AddColumn<bool>(
                name: "ContinueWhenError",
                table: "JobTaskDefinitions",
                nullable: true);
        }
    }
}
