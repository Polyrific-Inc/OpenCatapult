using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class RemoveRelationJobQueueDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobQueues_JobDefinitions_JobDefinitionId",
                table: "JobQueues");

            migrationBuilder.DropIndex(
                name: "IX_JobQueues_JobDefinitionId",
                table: "JobQueues");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeletion",
                table: "JobQueues",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobDefinitionName",
                table: "JobQueues",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeletion",
                table: "JobQueues");

            migrationBuilder.DropColumn(
                name: "JobDefinitionName",
                table: "JobQueues");

            migrationBuilder.CreateIndex(
                name: "IX_JobQueues_JobDefinitionId",
                table: "JobQueues",
                column: "JobDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobQueues_JobDefinitions_JobDefinitionId",
                table: "JobQueues",
                column: "JobDefinitionId",
                principalTable: "JobDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
