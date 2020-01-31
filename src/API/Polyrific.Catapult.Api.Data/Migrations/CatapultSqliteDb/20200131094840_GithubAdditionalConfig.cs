using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations.CatapultSqliteDb
{
    public partial class GithubAdditionalConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 18, null, "c48cafcc-b3e9-4375-a2c2-f30404382270", new DateTime(2020, 1, 31, 7, 23, 37, 58, DateTimeKind.Utc), "Do you want to skip the process of assigning project member to github project?", null, false, false, "Skip Project Member Configuration?", "SkipMemberConfig", 2, "boolean", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TaskProviderAdditionalConfigs",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
