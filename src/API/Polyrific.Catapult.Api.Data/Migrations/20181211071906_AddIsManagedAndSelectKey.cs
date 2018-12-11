using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddIsManagedAndSelectKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManaged",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectKey",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManaged",
                table: "ProjectDataModelProperties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManaged",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "SelectKey",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "IsManaged",
                table: "ProjectDataModelProperties");
        }
    }
}
