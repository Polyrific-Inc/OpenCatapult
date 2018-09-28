using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class FixGitHubProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExternalServiceProperties",
                keyColumns: new[] { "Id", "ConcurrencyStamp" },
                keyValues: new object[] { 8, "416fcf67-35cf-4ea3-b534-dade4a81da8b" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowedValues", "ConcurrencyStamp", "Description", "Name" },
                values: new object[] { "userPassword,authToken", "504200ee-f48a-4efa-be48-e09d16ee8d65", "Remote Credential Type (\"userPassword\" or \"authToken\")", "RemoteCredentialType" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowedValues", "ConcurrencyStamp", "Description", "IsRequired", "Name" },
                values: new object[] { null, "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", "Remote Username", false, "RemoteUsername" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", "Remote Password", true, "RemotePassword" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Description", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da88", "Repository Auth Token", "RepoAuthToken" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da89", "Application Id", 2, true, false, "ApplicationId" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8a", "Application Key", true, "ApplicationKey" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8b", "Tenant Id", false, "TenantId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AllowedValues", "ConcurrencyStamp", "Description", "Name" },
                values: new object[] { null, "bb36270b-654f-42bc-8508-c8bd0acafb5b", "Remote Url", "RemoteUrl" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AllowedValues", "ConcurrencyStamp", "Description", "IsRequired", "Name" },
                values: new object[] { "userPassword,authToken", "504200ee-f48a-4efa-be48-e09d16ee8d65", "Remote Credential Type (\"userPassword\" or \"authToken\")", true, "RemoteCredentialType" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", "Remote Username", false, "RemoteUsername" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Description", "Name" },
                values: new object[] { "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", "Remote Password", "RemotePassword" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da88", "Repository Auth Token", 1, false, true, "RepoAuthToken" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da89", "Application Id", false, "ApplicationId" });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Description", "IsSecret", "Name" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8a", "Application Key", true, "ApplicationKey" });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Updated" },
                values: new object[] { 8, null, "416fcf67-35cf-4ea3-b534-dade4a81da8b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Tenant Id", 2, true, false, "TenantId", null });
        }
    }
}
