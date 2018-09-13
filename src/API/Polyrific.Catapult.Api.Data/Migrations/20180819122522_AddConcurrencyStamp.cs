using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddConcurrencyStamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ProjectMembers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ProjectMemberRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ProjectDataModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ProjectDataModelProperties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "JobTaskDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "JobQueues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "JobDefinitions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "JobCounters",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ebe3a797-1758-4782-a77b-a78cd08433ea");

            migrationBuilder.UpdateData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "49db1ab1-9f16-4db0-b32d-5a916c2d39cd");

            migrationBuilder.UpdateData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "82dcaf01-bc5f-4964-b665-56074560861f");

            migrationBuilder.UpdateData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d25d2b9c-b2dc-4a36-99af-0622de434e83");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ProjectMemberRoles");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ProjectDataModels");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ProjectDataModelProperties");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "JobTaskDefinitions");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "JobQueues");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "JobDefinitions");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "JobCounters");
        }
    }
}
