using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "22521f64-d44d-4690-8b46-006528ae1c6e", "Administrator", "Administrator" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "48e22d3f-8d46-42b4-98fa-f7976fc15c2f", "Basic", "Basic" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3, "3aa1aafd-132b-4e9d-a9bd-0b4e2ce713ca", "Guest", "Guest" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 1, "22521f64-d44d-4690-8b46-006528ae1c6e" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 2, "48e22d3f-8d46-42b4-98fa-f7976fc15c2f" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 3, "3aa1aafd-132b-4e9d-a9bd-0b4e2ce713ca" });
        }
    }
}
