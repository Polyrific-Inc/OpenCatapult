using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UserRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "f8025fee-dec6-4528-9514-58339adc3383", "ADMINISTRATOR" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "c7cbed51-e910-4c2d-ab17-b27d3001ea47", "BASIC" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "18f44ef4-86b2-4ebb-a400-b2615c9715e0", "GUEST" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "6e60fade-1c1f-4f6a-ab7e-768358780783", "admin@opencatapult.net", true, false, null, "ADMIN@OPENCATAPULT.NET", "ADMIN@OPENCATAPULT.NET", "AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==", null, false, "D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK", false, "admin@opencatapult.net" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { 1, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 1, "6e60fade-1c1f-4f6a-ab7e-768358780783" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "22521f64-d44d-4690-8b46-006528ae1c6e", "Administrator" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "48e22d3f-8d46-42b4-98fa-f7976fc15c2f", "Basic" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "NormalizedName" },
                values: new object[] { "3aa1aafd-132b-4e9d-a9bd-0b4e2ce713ca", "Guest" });
        }
    }
}
