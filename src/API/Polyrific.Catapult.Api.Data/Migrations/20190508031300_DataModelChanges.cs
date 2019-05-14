using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class DataModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatabaseTableName",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DatabaseColumnName",
                table: "ProjectDataModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsKey",
                table: "ProjectDataModelProperties",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ProjectDataModelProperties",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseTableName",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "DatabaseColumnName",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "IsKey",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ProjectDataModelProperties");
        }
    }
}
