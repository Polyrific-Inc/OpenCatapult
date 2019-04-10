using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UpdateTaskProviderProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Plugins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Plugins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Plugins",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
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
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluginTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PluginId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PluginTags_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PluginTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a1", "A generator task provider for generating an application with AspNet Core Mvc and Entity Framework Core backend", "AspNet Core Mvc Generator", "/assets/img/task-provider/aspnetcore.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a2", "A repository task provider for cloning, pushing code, creating new branch and pull request into a GitHub repository", "GitHub Repository", "/assets/img/task-provider/github.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a3", "A build task provider for building & publishing dotnet core application", "DotNet Core Build", "/assets/img/task-provider/dotnetcore.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a4", "A test task provider for running a unit test project using \"dotnet test\" command", "Dotnet Core Test", "/assets/img/task-provider/dotnetcore.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a5", "A Deploy database task provider for running the migration script with entity framework core to a designated database", "Entity Framework Core Database Migrator", "/assets/img/task-provider/efcore.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a6", "A deploy task provider for deploying an application to azure app service", "Deploy To Azure App Service", "/assets/img/task-provider/azureappservice.png" });

            migrationBuilder.UpdateData(
                table: "Plugins",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Description", "DisplayName", "ThumbnailUrl" },
                values: new object[] { "976e0533-360a-4e46-8220-7c1cfdf0e0a7", "A generic task provider for running any command in a preferred command line tools such as powershell or bash", "Generic Command", "/assets/img/task-provider/generic.png" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[,]
                {
                    { 17, "7c29af83-c493-4f23-a600-e5f9d1d2bc4f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Git", null },
                    { 18, "7c29af83-c493-4f23-a600-e5f9d1d2bc50", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Unit Test", null },
                    { 19, "7c29af83-c493-4f23-a600-e5f9d1d2bc51", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "xUnit", null },
                    { 20, "7c29af83-c493-4f23-a600-e5f9d1d2bc52", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Azure", null },
                    { 21, "7c29af83-c493-4f23-a600-e5f9d1d2bc53", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Azure App Service", null },
                    { 22, "7c29af83-c493-4f23-a600-e5f9d1d2bc54", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Hosting", null },
                    { 25, "7c29af83-c493-4f23-a600-e5f9d1d2bc357", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Generic", null },
                    { 24, "7c29af83-c493-4f23-a600-e5f9d1d2bc56", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "PaaS", null },
                    { 16, "7c29af83-c493-4f23-a600-e5f9d1d2bc4e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Source Control", null },
                    { 26, "7c29af83-c493-4f23-a600-e5f9d1d2bc58", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Command", null },
                    { 27, "7c29af83-c493-4f23-a600-e5f9d1d2bc59", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Command Line", null },
                    { 28, "7c29af83-c493-4f23-a600-e5f9d1d2bc5a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "CLI", null },
                    { 23, "7c29af83-c493-4f23-a600-e5f9d1d2bc55", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Cloud", null },
                    { 15, "7c29af83-c493-4f23-a600-e5f9d1d2bc4c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "GitHub", null },
                    { 12, "7c29af83-c493-4f23-a600-e5f9d1d2bc49", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Web", null },
                    { 13, "7c29af83-c493-4f23-a600-e5f9d1d2bc4a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "CRUD", null },
                    { 29, "7c29af83-c493-4f23-a600-e5f9d1d2bc5b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Tool", null },
                    { 11, "7c29af83-c493-4f23-a600-e5f9d1d2bc48", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "MVC", null },
                    { 10, "7c29af83-c493-4f23-a600-e5f9d1d2bc47", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "AspNet Core Mvc", null },
                    { 9, "7c29af83-c493-4f23-a600-e5f9d1d2bc46", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "AspNet Core", null },
                    { 8, "7c29af83-c493-4f23-a600-e5f9d1d2bc45", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Microsoft", null },
                    { 7, "7c29af83-c493-4f23-a600-e5f9d1d2bc44", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "DotNet Core", null },
                    { 6, "7c29af83-c493-4f23-a600-e5f9d1d2bc43", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Deploy", null },
                    { 5, "7c29af83-c493-4f23-a600-e5f9d1d2bc42", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Database Deploy", null },
                    { 4, "7c29af83-c493-4f23-a600-e5f9d1d2bc41", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Test", null },
                    { 3, "7c29af83-c493-4f23-a600-e5f9d1d2bc40", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Build", null },
                    { 2, "7c29af83-c493-4f23-a600-e5f9d1d2bc3f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Repository", null },
                    { 1, "7c29af83-c493-4f23-a600-e5f9d1d2bc3e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Code Generator", null },
                    { 14, "7c29af83-c493-4f23-a600-e5f9d1d2bc4b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Entity Framework Core", null },
                    { 30, "7c29af83-c493-4f23-a600-e5f9d1d2bc5c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Custom", null }
                });

            migrationBuilder.InsertData(
                table: "PluginTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "PluginId", "TagId", "Updated" },
                values: new object[,]
                {
                    { 1, "21222bae-5e15-432c-ae4f-e671cb116d07", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 1, null },
                    { 24, "21222bae-5e15-432c-ae4f-e671cb116d1f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 14, null },
                    { 11, "21222bae-5e15-432c-ae4f-e671cb116d11", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 15, null },
                    { 12, "21222bae-5e15-432c-ae4f-e671cb116d12", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 16, null },
                    { 13, "21222bae-5e15-432c-ae4f-e671cb116d13", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 17, null },
                    { 19, "21222bae-5e15-432c-ae4f-e671cb116d19", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 18, null },
                    { 20, "21222bae-5e15-432c-ae4f-e671cb116d1a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 19, null },
                    { 28, "21222bae-5e15-432c-ae4f-e671cb116d23", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 20, null },
                    { 29, "21222bae-5e15-432c-ae4f-e671cb116d24", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 21, null },
                    { 30, "21222bae-5e15-432c-ae4f-e671cb116d25", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 22, null },
                    { 31, "21222bae-5e15-432c-ae4f-e671cb116d26", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 23, null },
                    { 32, "21222bae-5e15-432c-ae4f-e671cb116d27", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 24, null },
                    { 33, "21222bae-5e15-432c-ae4f-e671cb116d28", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 25, null },
                    { 34, "21222bae-5e15-432c-ae4f-e671cb116d29", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 26, null },
                    { 35, "21222bae-5e15-432c-ae4f-e671cb116d2a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 27, null },
                    { 36, "21222bae-5e15-432c-ae4f-e671cb116d2b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 28, null },
                    { 9, "21222bae-5e15-432c-ae4f-e671cb116d0f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 14, null },
                    { 8, "21222bae-5e15-432c-ae4f-e671cb116d0e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 13, null },
                    { 27, "21222bae-5e15-432c-ae4f-e671cb116d22", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 12, null },
                    { 7, "21222bae-5e15-432c-ae4f-e671cb116d0d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 12, null },
                    { 10, "21222bae-5e15-432c-ae4f-e671cb116d10", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 2, null },
                    { 14, "21222bae-5e15-432c-ae4f-e671cb116d14", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 3, null },
                    { 17, "21222bae-5e15-432c-ae4f-e671cb116d17", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 4, null },
                    { 21, "21222bae-5e15-432c-ae4f-e671cb116d1b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 5, null },
                    { 25, "21222bae-5e15-432c-ae4f-e671cb116d20", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 6, null },
                    { 2, "21222bae-5e15-432c-ae4f-e671cb116d08", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 7, null },
                    { 15, "21222bae-5e15-432c-ae4f-e671cb116d15", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 7, null },
                    { 37, "21222bae-5e15-432c-ae4f-e671cb116d2c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 29, null },
                    { 18, "21222bae-5e15-432c-ae4f-e671cb116d18", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 7, null },
                    { 3, "21222bae-5e15-432c-ae4f-e671cb116d09", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 8, null },
                    { 16, "21222bae-5e15-432c-ae4f-e671cb116d16", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 8, null },
                    { 23, "21222bae-5e15-432c-ae4f-e671cb116d1d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 8, null },
                    { 26, "21222bae-5e15-432c-ae4f-e671cb116d21", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 8, null },
                    { 4, "21222bae-5e15-432c-ae4f-e671cb116d0a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 9, null },
                    { 5, "21222bae-5e15-432c-ae4f-e671cb116d0b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 10, null },
                    { 6, "21222bae-5e15-432c-ae4f-e671cb116d0c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 11, null },
                    { 22, "21222bae-5e15-432c-ae4f-e671cb116d1c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 7, null },
                    { 38, "21222bae-5e15-432c-ae4f-e671cb116d2d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 30, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginTags_PluginId",
                table: "PluginTags",
                column: "PluginId");

            migrationBuilder.CreateIndex(
                name: "IX_PluginTags_TagId",
                table: "PluginTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Plugins");
        }
    }
}
