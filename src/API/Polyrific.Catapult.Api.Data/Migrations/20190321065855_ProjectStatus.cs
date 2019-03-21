using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class ProjectStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Projects",
                nullable: false,
                defaultValue: "active");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projects");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Projects",
                nullable: false,
                defaultValue: false);
        }
    }
}
