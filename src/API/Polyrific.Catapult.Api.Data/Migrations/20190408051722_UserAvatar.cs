using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations
{
    public partial class UserAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarFileId",
                table: "UserProfile",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManagedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagedFiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_AvatarFileId",
                table: "UserProfile",
                column: "AvatarFileId",
                unique: true,
                filter: "[AvatarFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_ManagedFiles_AvatarFileId",
                table: "UserProfile",
                column: "AvatarFileId",
                principalTable: "ManagedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_ManagedFiles_AvatarFileId",
                table: "UserProfile");

            migrationBuilder.DropTable(
                name: "ManagedFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_AvatarFileId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "AvatarFileId",
                table: "UserProfile");
        }
    }
}
