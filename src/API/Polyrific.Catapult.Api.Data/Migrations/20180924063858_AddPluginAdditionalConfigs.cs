using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class AddPluginAdditionalConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PluginAdditionalConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PluginId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginAdditionalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PluginAdditionalConfigs_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginAdditionalConfigs_PluginId",
                table: "PluginAdditionalConfigs",
                column: "PluginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginAdditionalConfigs");
        }
    }
}
