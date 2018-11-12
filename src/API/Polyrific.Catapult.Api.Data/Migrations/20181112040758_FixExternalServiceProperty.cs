using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class FixExternalServiceProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "504200ee-f48a-4efa-be48-e09d16ee8d65", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da88", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da89", 3 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8a", 3 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8b", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "504200ee-f48a-4efa-be48-e09d16ee8d65", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da88", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da89", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8a", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "ExternalServiceTypeId" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8b", 2 });
        }
    }
}
