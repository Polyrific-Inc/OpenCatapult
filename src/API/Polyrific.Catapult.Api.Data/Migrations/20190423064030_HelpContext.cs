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
                    Section = table.Column<string>(nullable: true),
                    SubSection = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpContexts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[,]
                {
                    { 1, "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, null, null },
                    { 21, "504200ee-f48a-4efa-be48-e09d16ee8d7f", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "User", 0, null, null },
                    { 20, "504200ee-f48a-4efa-be48-e09d16ee8d7e", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "TaskProvider", 0, null, null },
                    { 19, "504200ee-f48a-4efa-be48-e09d16ee8d7d", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Engine", 0, "Engine Token", null },
                    { 18, "504200ee-f48a-4efa-be48-e09d16ee8d7c", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Engine", 0, null, null },
                    { 17, "504200ee-f48a-4efa-be48-e09d16ee8d7b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ExternalService", 0, "External Service Type", null },
                    { 16, "504200ee-f48a-4efa-be48-e09d16ee8d7a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ExternalService", 0, null, null },
                    { 15, "504200ee-f48a-4efa-be48-e09d16ee8d79", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectMember", 0, "Project Role", null },
                    { 14, "504200ee-f48a-4efa-be48-e09d16ee8d78", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectMember", 0, null, null },
                    { 13, "504200ee-f48a-4efa-be48-e09d16ee8d77", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, "Detail", null },
                    { 22, "504200ee-f48a-4efa-be48-e09d16ee8d80", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "User", 0, "User Role", null },
                    { 12, "504200ee-f48a-4efa-be48-e09d16ee8d76", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, "Logs", null },
                    { 10, "504200ee-f48a-4efa-be48-e09d16ee8d74", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobTaskDefinition", 0, null, null },
                    { 9, "504200ee-f48a-4efa-be48-e09d16ee8d73", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, "Job Queue", null },
                    { 8, "504200ee-f48a-4efa-be48-e09d16ee8d72", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, "Job Task", null },
                    { 7, "504200ee-f48a-4efa-be48-e09d16ee8d71", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, null, null },
                    { 6, "504200ee-f48a-4efa-be48-e09d16ee8d70", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModelProperty", 0, null, null },
                    { 5, "504200ee-f48a-4efa-be48-e09d16ee8d69", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModel", 0, "Properties", null },
                    { 4, "504200ee-f48a-4efa-be48-e09d16ee8d68", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModel", 0, null, null },
                    { 3, "504200ee-f48a-4efa-be48-e09d16ee8d67", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, "Project List", null },
                    { 2, "504200ee-f48a-4efa-be48-e09d16ee8d66", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, "Create Project", null },
                    { 11, "504200ee-f48a-4efa-be48-e09d16ee8d75", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, null, null },
                    { 23, "504200ee-f48a-4efa-be48-e09d16ee8d81", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "UserProfile", 0, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpContexts");
        }
    }
}
