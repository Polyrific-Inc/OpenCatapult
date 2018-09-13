using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddMissingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Client",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControlType",
                table: "ProjectDataModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedProjectDataModelId",
                table: "ProjectDataModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelationalType",
                table: "ProjectDataModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JobTaskDefinitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDataModelProperties_RelatedProjectDataModelId",
                table: "ProjectDataModelProperties",
                column: "RelatedProjectDataModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDataModelProperties_ProjectDataModels_RelatedProjectDataModelId",
                table: "ProjectDataModelProperties",
                column: "RelatedProjectDataModelId",
                principalTable: "ProjectDataModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDataModelProperties_ProjectDataModels_RelatedProjectDataModelId",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDataModelProperties_RelatedProjectDataModelId",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "Client",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "ControlType",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "RelatedProjectDataModelId",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "RelationalType",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "JobTaskDefinitions");
        }
    }
}
