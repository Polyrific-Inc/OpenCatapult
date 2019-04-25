using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UserExternalAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalAccountIds",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalAccountType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ExternalAccountType",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Key", "Label", "Updated" },
                values: new object[] { 1, "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "GitHub", "GitHub Id", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAccountType");

            migrationBuilder.DropColumn(
                name: "ExternalAccountIds",
                table: "UserProfile");
        }
    }
}
