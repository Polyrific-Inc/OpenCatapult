using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class IsActiveUserAndEngine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserProfile",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CatapultEngineProfile",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Id", "ApplicationUserId", "ConcurrencyStamp", "Created", "FirstName", "IsActive", "LastName", "Updated" },
                values: new object[] { 1, 1, "99aa6fde-2675-4aa9-a60d-e45ba72fb9d0", new DateTime(2018, 8, 23, 10, 4, 6, 797, DateTimeKind.Utc), null, true, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserProfile",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CatapultEngineProfile");
        }
    }
}
