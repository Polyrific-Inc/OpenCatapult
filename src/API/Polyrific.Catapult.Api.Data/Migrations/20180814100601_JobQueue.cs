using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class JobQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCounters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobQueues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    CatapultEngineId = table.Column<string>(nullable: true),
                    JobType = table.Column<string>(nullable: true),
                    CatapultEngineMachineName = table.Column<string>(nullable: true),
                    CatapultEngineIPAddress = table.Column<string>(nullable: true),
                    CatapultEngineVersion = table.Column<string>(nullable: true),
                    OriginUrl = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    JobDefinitionId = table.Column<int>(nullable: true),
                    JobTasksStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobQueues_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobQueues_ProjectId",
                table: "JobQueues",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobCounters");

            migrationBuilder.DropTable(
                name: "JobQueues");
        }
    }
}
