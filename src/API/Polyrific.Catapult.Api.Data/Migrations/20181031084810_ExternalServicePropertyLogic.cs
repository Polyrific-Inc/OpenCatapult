using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class ExternalServicePropertyLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalLogic",
                table: "ExternalServiceProperties",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "ExternalServiceProperties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Sequence" },
                values: new object[] { "504200ee-f48a-4efa-be48-e09d16ee8d65", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AdditionalLogic", "ConcurrencyStamp", "Sequence" },
                values: new object[] { "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }", "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AdditionalLogic", "ConcurrencyStamp", "Sequence" },
                values: new object[] { "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }", "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", 3 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AdditionalLogic", "ConcurrencyStamp", "Sequence" },
                values: new object[] { "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" } }", "416fcf67-35cf-4ea3-b534-dade4a81da88", 4 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "Sequence" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da89", 1 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ConcurrencyStamp", "Sequence" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8a", 2 });

            migrationBuilder.UpdateData(
                table: "ExternalServiceProperties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ConcurrencyStamp", "Sequence" },
                values: new object[] { "416fcf67-35cf-4ea3-b534-dade4a81da8b", 3 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobQueues_JobDefinitions_JobDefinitionId",
                table: "JobQueues");

            migrationBuilder.DropIndex(
                name: "IX_JobQueues_JobDefinitionId",
                table: "JobQueues");

            migrationBuilder.DropColumn(
                name: "AdditionalLogic",
                table: "ExternalServiceProperties");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "ExternalServiceProperties");
        }
    }
}
