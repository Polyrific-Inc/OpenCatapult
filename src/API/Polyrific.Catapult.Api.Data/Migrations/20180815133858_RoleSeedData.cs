using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RoleSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProjectMemberRoles",
                columns: new[] { "Id", "Created", "Name", "Updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Owner", null },
                    { 2, new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Maintainer", null },
                    { 3, new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Contributor", null },
                    { 4, new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Member", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 4, "0c810611-1e85-47cc-a7a1-7c57ff3e29bb", "Engine", "ENGINE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProjectMemberRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 4, "0c810611-1e85-47cc-a7a1-7c57ff3e29bb" });
        }
    }
}
