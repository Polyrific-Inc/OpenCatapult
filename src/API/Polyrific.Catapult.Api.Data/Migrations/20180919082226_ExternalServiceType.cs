using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class ExternalServiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ExternalServices");

            migrationBuilder.AddColumn<int>(
                name: "ExternalServiceTypeId",
                table: "ExternalServices",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalServiceProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AllowedValues = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false),
                    ExternalServiceTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalServiceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalServiceProperties_ExternalServiceTypes_ExternalServiceTypeId",
                        column: x => x.ExternalServiceTypeId,
                        principalTable: "ExternalServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 1, "2425fe0d-4e3e-4549-a9a7-60056097ce96", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "GitHub", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Updated" },
                values: new object[,]
                {
                    { 1, null, "bb36270b-654f-42bc-8508-c8bd0acafb5b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Url", 1, true, false, "RemoteUrl", null },
                    { 2, "userPassword,authToken", "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Credential Type (\"userPassword\" or \"authToken\")", 1, true, false, "RemoteCredentialType", null },
                    { 3, null, "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Username", 1, false, false, "RemoteUsername", null },
                    { 4, null, "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Password", 1, false, true, "RemotePassword", null },
                    { 5, null, "416fcf67-35cf-4ea3-b534-dade4a81da88", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Repository Auth Token", 1, false, true, "RepoAuthToken", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalServices_ExternalServiceTypeId",
                table: "ExternalServices",
                column: "ExternalServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalServiceProperties_ExternalServiceTypeId",
                table: "ExternalServiceProperties",
                column: "ExternalServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalServices_ExternalServiceTypes_ExternalServiceTypeId",
                table: "ExternalServices",
                column: "ExternalServiceTypeId",
                principalTable: "ExternalServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalServices_ExternalServiceTypes_ExternalServiceTypeId",
                table: "ExternalServices");

            migrationBuilder.DropTable(
                name: "ExternalServiceProperties");

            migrationBuilder.DropTable(
                name: "ExternalServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ExternalServices_ExternalServiceTypeId",
                table: "ExternalServices");

            migrationBuilder.DropColumn(
                name: "ExternalServiceTypeId",
                table: "ExternalServices");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ExternalServices",
                nullable: true);
        }
    }
}
