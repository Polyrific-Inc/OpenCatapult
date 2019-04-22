using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class HelpContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelpContexts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Section = table.Column<int>(nullable: false),
                    SubSection = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpContexts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Text", "Updated" },
                values: new object[] { 1, "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), 1, 0, null, "Project is a unit of work which your team will work with to create an application", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Text", "Updated" },
                values: new object[] { 2, "504200ee-f48a-4efa-be48-e09d16ee8d66", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), 1, 0, "Create Project", "Start to create your own project which have its own model and job definition", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Text", "Updated" },
                values: new object[] { 3, "504200ee-f48a-4efa-be48-e09d16ee8d67", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), 1, 0, "Project List", "The list of project is shown based on your access depending on your project membership. An administrator will have access to all projects.", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpContexts");
        }
    }
}
