using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Polyrific.Catapult.Api.Data.Migrations.CatapultSqliteDb
{
    public partial class SqliteInitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateTable(
                name: "ExternalServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HelpContexts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateTable(
                name: "JobCounters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ManagedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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

            migrationBuilder.CreateTable(
                name: "ProjectMemberRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Client = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false, defaultValue: "active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                name: "TaskProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    RequiredServicesString = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    IsCatapultEngine = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalServiceProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AllowedValues = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false),
                    AdditionalLogic = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    ExternalServiceTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalServiceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalServiceProperties_ExternalServiceTypes_ExternalServiceTypeId",
                        column: x => x.ExternalServiceTypeId,
                        principalTable: "ExternalServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalServices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    ExternalServiceTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalServices_ExternalServiceTypes_ExternalServiceTypeId",
                        column: x => x.ExternalServiceTypeId,
                        principalTable: "ExternalServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeletion = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDefinitions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobQueues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
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
                    JobDefinitionName = table.Column<string>(nullable: true),
                    IsDeletion = table.Column<bool>(nullable: false),
                    JobTasksStatus = table.Column<string>(nullable: true),
                    OutputValues = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ProjectDataModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    IsManaged = table.Column<bool>(nullable: true),
                    SelectKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDataModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDataModels_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskProviderAdditionalConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TaskProviderId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsSecret = table.Column<bool>(nullable: false),
                    IsInputMasked = table.Column<bool>(nullable: true),
                    AllowedValues = table.Column<string>(nullable: true),
                    Hint = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviderAdditionalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskProviderAdditionalConfigs_TaskProviders_TaskProviderId",
                        column: x => x.TaskProviderId,
                        principalTable: "TaskProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskProviderTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TaskProviderId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskProviderTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskProviderTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskProviderTags_TaskProviders_TaskProviderId",
                        column: x => x.TaskProviderId,
                        principalTable: "TaskProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatapultEngineProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    LastSeen = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    Version = table.Column<string>(nullable: true),
                    CatapultEngineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatapultEngineProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatapultEngineProfile_Users_CatapultEngineId",
                        column: x => x.CatapultEngineId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ProjectMemberRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_ProjectMemberRoles_ProjectMemberRoleId",
                        column: x => x.ProjectMemberRoleId,
                        principalTable: "ProjectMemberRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    ApplicationUserId = table.Column<int>(nullable: true),
                    AvatarFileId = table.Column<int>(nullable: true),
                    ExternalAccountIds = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfile_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfile_ManagedFiles_AvatarFileId",
                        column: x => x.AvatarFileId,
                        principalTable: "ManagedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobTaskDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    JobDefinitionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Provider = table.Column<string>(nullable: true),
                    ConfigString = table.Column<string>(nullable: true),
                    AdditionalConfigString = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTaskDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTaskDefinitions_JobDefinitions_JobDefinitionId",
                        column: x => x.JobDefinitionId,
                        principalTable: "JobDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDataModelProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ProjectDataModelId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    RelatedProjectDataModelId = table.Column<int>(nullable: true),
                    RelationalType = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    IsManaged = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDataModelProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDataModelProperties_ProjectDataModels_ProjectDataModelId",
                        column: x => x.ProjectDataModelId,
                        principalTable: "ProjectDataModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectDataModelProperties_ProjectDataModels_RelatedProjectDataModelId",
                        column: x => x.RelatedProjectDataModelId,
                        principalTable: "ProjectDataModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ExternalAccountType",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Key", "Label", "Updated" },
                values: new object[] { 1, "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "GitHub", "GitHub Id", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 1, "2425fe0d-4e3e-4549-a9a7-60056097ce98", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "Generic", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 2, "2425fe0d-4e3e-4549-a9a7-60056097ce96", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "GitHub", null });

            migrationBuilder.InsertData(
                table: "ExternalServiceTypes",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 3, "2425fe0d-4e3e-4549-a9a7-60056097ce97", new DateTime(2018, 9, 19, 8, 14, 52, 51, DateTimeKind.Utc), "Azure", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 23, "504200ee-f48a-4efa-be48-e09d16ee8d81", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "UserProfile", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 22, "504200ee-f48a-4efa-be48-e09d16ee8d80", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "User", 0, "User Role", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 21, "504200ee-f48a-4efa-be48-e09d16ee8d7f", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "User", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 19, "504200ee-f48a-4efa-be48-e09d16ee8d7d", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Engine", 0, "Engine Token", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 18, "504200ee-f48a-4efa-be48-e09d16ee8d7c", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Engine", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 17, "504200ee-f48a-4efa-be48-e09d16ee8d7b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ExternalService", 0, "External Service Type", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 16, "504200ee-f48a-4efa-be48-e09d16ee8d7a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ExternalService", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 15, "504200ee-f48a-4efa-be48-e09d16ee8d79", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectMember", 0, "Project Role", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 14, "504200ee-f48a-4efa-be48-e09d16ee8d78", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectMember", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 13, "504200ee-f48a-4efa-be48-e09d16ee8d77", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, "Detail", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 12, "504200ee-f48a-4efa-be48-e09d16ee8d76", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, "Logs", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 20, "504200ee-f48a-4efa-be48-e09d16ee8d7e", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "TaskProvider", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 10, "504200ee-f48a-4efa-be48-e09d16ee8d74", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobTaskDefinition", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 1, "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 2, "504200ee-f48a-4efa-be48-e09d16ee8d66", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, "Create Project", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 11, "504200ee-f48a-4efa-be48-e09d16ee8d75", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobQueue", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 4, "504200ee-f48a-4efa-be48-e09d16ee8d68", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModel", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 3, "504200ee-f48a-4efa-be48-e09d16ee8d67", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Project", 0, "Project List", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 6, "504200ee-f48a-4efa-be48-e09d16ee8d70", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModelProperty", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 7, "504200ee-f48a-4efa-be48-e09d16ee8d71", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, null, null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 8, "504200ee-f48a-4efa-be48-e09d16ee8d72", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, "Job Task", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 9, "504200ee-f48a-4efa-be48-e09d16ee8d73", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "JobDefinition", 0, "Job Queue", null });

            migrationBuilder.InsertData(
                table: "HelpContexts",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Section", "Sequence", "SubSection", "Updated" },
                values: new object[] { 5, "504200ee-f48a-4efa-be48-e09d16ee8d69", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "ProjectModel", 0, "Properties", null });

            migrationBuilder.InsertData(
                table: "ProjectMemberRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 1, "ebe3a797-1758-4782-a77b-a78cd08433ea", new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Owner", null });

            migrationBuilder.InsertData(
                table: "ProjectMemberRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 2, "49db1ab1-9f16-4db0-b32d-5a916c2d39cd", new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Maintainer", null });

            migrationBuilder.InsertData(
                table: "ProjectMemberRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 3, "82dcaf01-bc5f-4964-b665-56074560861f", new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Contributor", null });

            migrationBuilder.InsertData(
                table: "ProjectMemberRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 4, "d25d2b9c-b2dc-4a36-99af-0622de434e83", new DateTime(2018, 8, 15, 13, 38, 58, 310, DateTimeKind.Utc), "Member", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 4, "0c810611-1e85-47cc-a7a1-7c57ff3e29bb", "Engine", "ENGINE" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "f8025fee-dec6-4528-9514-58339adc3383", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 3, "18f44ef4-86b2-4ebb-a400-b2615c9715e0", "Guest", "GUEST" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "c7cbed51-e910-4c2d-ab17-b27d3001ea47", "Basic", "BASIC" });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 30, "7c29af83-c493-4f23-a600-e5f9d1d2bc5c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Custom", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 29, "7c29af83-c493-4f23-a600-e5f9d1d2bc5b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Tool", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 19, "7c29af83-c493-4f23-a600-e5f9d1d2bc51", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "xUnit", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 28, "7c29af83-c493-4f23-a600-e5f9d1d2bc5a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "CLI", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 27, "7c29af83-c493-4f23-a600-e5f9d1d2bc59", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Command Line", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 26, "7c29af83-c493-4f23-a600-e5f9d1d2bc58", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Command", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 25, "7c29af83-c493-4f23-a600-e5f9d1d2bc357", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Generic", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 24, "7c29af83-c493-4f23-a600-e5f9d1d2bc56", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "PaaS", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 23, "7c29af83-c493-4f23-a600-e5f9d1d2bc55", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Cloud", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 22, "7c29af83-c493-4f23-a600-e5f9d1d2bc54", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Hosting", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 21, "7c29af83-c493-4f23-a600-e5f9d1d2bc53", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Azure App Service", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 20, "7c29af83-c493-4f23-a600-e5f9d1d2bc52", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Azure", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 18, "7c29af83-c493-4f23-a600-e5f9d1d2bc50", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Unit Test", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 6, "7c29af83-c493-4f23-a600-e5f9d1d2bc43", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Deploy", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 16, "7c29af83-c493-4f23-a600-e5f9d1d2bc4e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Source Control", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 1, "7c29af83-c493-4f23-a600-e5f9d1d2bc3e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Code Generator", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 2, "7c29af83-c493-4f23-a600-e5f9d1d2bc3f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Repository", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 3, "7c29af83-c493-4f23-a600-e5f9d1d2bc40", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Build", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 17, "7c29af83-c493-4f23-a600-e5f9d1d2bc4f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Git", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 5, "7c29af83-c493-4f23-a600-e5f9d1d2bc42", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Database Deploy", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 7, "7c29af83-c493-4f23-a600-e5f9d1d2bc44", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "DotNet Core", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 8, "7c29af83-c493-4f23-a600-e5f9d1d2bc45", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Microsoft", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 4, "7c29af83-c493-4f23-a600-e5f9d1d2bc41", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Test", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 10, "7c29af83-c493-4f23-a600-e5f9d1d2bc47", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "AspNet Core Mvc", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 11, "7c29af83-c493-4f23-a600-e5f9d1d2bc48", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "MVC", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 12, "7c29af83-c493-4f23-a600-e5f9d1d2bc49", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Web", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 13, "7c29af83-c493-4f23-a600-e5f9d1d2bc4a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "CRUD", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 14, "7c29af83-c493-4f23-a600-e5f9d1d2bc4b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "Entity Framework Core", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 15, "7c29af83-c493-4f23-a600-e5f9d1d2bc4c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "GitHub", null });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "Name", "Updated" },
                values: new object[] { 9, "7c29af83-c493-4f23-a600-e5f9d1d2bc46", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), "AspNet Core", null });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 1, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a1", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generator task provider for generating an application with AspNet Core Mvc and Entity Framework Core backend", "AspNet Core Mvc Generator", "Polyrific.Catapult.TaskProviders.AspNetCoreMvc", null, "/assets/img/task-provider/aspnetcore.png", "GeneratorProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 2, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a2", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A repository task provider for cloning, pushing code, creating new branch and pull request into a GitHub repository", "GitHub Repository", "Polyrific.Catapult.TaskProviders.GitHub", "GitHub", "/assets/img/task-provider/github.png", "RepositoryProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 3, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a3", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A build task provider for building & publishing dotnet core application", "DotNet Core Build", "Polyrific.Catapult.TaskProviders.DotNetCore", null, "/assets/img/task-provider/dotnetcore.png", "BuildProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 4, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a4", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A test task provider for running a unit test project using \"dotnet test\" command", "Dotnet Core Test", "Polyrific.Catapult.TaskProviders.DotNetCoreTest", null, "/assets/img/task-provider/dotnetcore.png", "TestProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 5, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a5", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A Deploy database task provider for running the migration script with entity framework core to a designated database", "Entity Framework Core Database Migrator", "Polyrific.Catapult.TaskProviders.EntityFrameworkCore", null, "/assets/img/task-provider/efcore.png", "DatabaseProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 6, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a6", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A deploy task provider for deploying an application to azure app service", "Deploy To Azure App Service", "Polyrific.Catapult.TaskProviders.AzureAppService", "Azure", "/assets/img/task-provider/azureappservice.png", "HostingProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "TaskProviders",
                columns: new[] { "Id", "Author", "ConcurrencyStamp", "Created", "Description", "DisplayName", "Name", "RequiredServicesString", "ThumbnailUrl", "Type", "Updated", "Version" },
                values: new object[] { 7, "Polyrific", "976e0533-360a-4e46-8220-7c1cfdf0e0a7", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "A generic task provider for running any command in a preferred command line tools such as powershell or bash", "Generic Command", "Polyrific.Catapult.TaskProviders.GenericCommand", null, "/assets/img/task-provider/generic.png", "GenericTaskProvider", null, "1.0.0-beta4" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsCatapultEngine", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "6e60fade-1c1f-4f6a-ab7e-768358780783", "admin@opencatapult.net", true, null, false, null, "ADMIN@OPENCATAPULT.NET", "ADMIN@OPENCATAPULT.NET", "AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==", null, false, "D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK", false, "admin@opencatapult.net" });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 1, null, "userPassword,authToken", "504200ee-f48a-4efa-be48-e09d16ee8d65", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Credential Type (\"userPassword\" or \"authToken\")", 2, true, false, "RemoteCredentialType", 1, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 2, "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }", null, "4bd86c55-ffc1-4c49-a4e4-c1ee809f311d", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Username", 2, false, false, "RemoteUsername", 2, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 3, "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" } }", null, "c1eeaa4b-bdc2-4ef9-a52d-393fe9dca59a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Remote Password", 2, false, true, "RemotePassword", 3, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 4, "{\"HideCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"userPassword\" }, \"RequiredCondition\": { \"PropertyName\": \"RemoteCredentialType\", \"PropertyValue\": \"authToken\" } }", null, "416fcf67-35cf-4ea3-b534-dade4a81da88", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Repository Auth Token", 2, false, true, "RepoAuthToken", 4, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 5, null, null, "416fcf67-35cf-4ea3-b534-dade4a81da89", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Application Id", 3, true, false, "ApplicationId", 1, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 6, null, null, "416fcf67-35cf-4ea3-b534-dade4a81da8a", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Application Key", 3, true, true, "ApplicationKey", 2, null });

            migrationBuilder.InsertData(
                table: "ExternalServiceProperties",
                columns: new[] { "Id", "AdditionalLogic", "AllowedValues", "ConcurrencyStamp", "Created", "Description", "ExternalServiceTypeId", "IsRequired", "IsSecret", "Name", "Sequence", "Updated" },
                values: new object[] { 7, null, null, "416fcf67-35cf-4ea3-b534-dade4a81da8b", new DateTime(2018, 9, 19, 8, 14, 52, 52, DateTimeKind.Utc), "Tenant Id", 3, true, false, "TenantId", 3, null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 6, null, "c48cafcc-b3e9-4375-a2c2-f30404382262", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, false, true, true, "Connection String", "ConnectionString", 5, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 7, null, "c48cafcc-b3e9-4375-a2c2-f3040438225e", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Subscription Id", "SubscriptionId", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 8, null, "c48cafcc-b3e9-4375-a2c2-f3040438225f", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, true, false, "Resource Group", "ResourceGroupName", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 9, null, "c48cafcc-b3e9-4375-a2c2-f30404382260", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "App Service", "AppServiceName", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 10, null, "c48cafcc-b3e9-4375-a2c2-f30404382266", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Do you want to automatically reassign app service name when it is not available?", null, false, false, "Allow Automatic Rename?", "AllowAutomaticRename", 6, "boolean", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 11, null, "c48cafcc-b3e9-4375-a2c2-f30404382261", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Deployment Slot", "DeploymentSlot", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 14, null, "c48cafcc-b3e9-4375-a2c2-f30404382265", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default App Service Plan", "AppServicePlan", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 13, null, "c48cafcc-b3e9-4375-a2c2-f30404382264", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Default Region", "Region", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 4, null, "c48cafcc-b3e9-4375-a2c2-f3040438225b", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Startup Project Name", "StartupProjectName", 5, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 15, null, "c48cafcc-b3e9-4375-a2c2-f30404382267", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "The command tool to be used to run the command (e.g. Powershell). Defaults based on OS.", null, false, false, "Command Tool", "CommandTool", 7, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 16, null, "c48cafcc-b3e9-4375-a2c2-f30404382268", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Command Text", "CommandText", 7, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 17, null, "c48cafcc-b3e9-4375-a2c2-f30404382269", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "You can provide a script file (it is recommended to use this if the input contains multiple lines of commands)", null, false, false, "Command Script Path", "CommandScriptPath", 7, "file", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 12, null, "c48cafcc-b3e9-4375-a2c2-f30404382263", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please make sure to enter the connection string if the website needs to connect to the database", false, false, true, "Connection String", "ConnectionString", 6, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 3, null, "c48cafcc-b3e9-4375-a2c2-f3040438225a", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Configuration", "Configuration", 3, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 5, null, "c48cafcc-b3e9-4375-a2c2-f3040438225c", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Database Project Name", "DatabaseProjectName", 5, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 2, null, "c48cafcc-b3e9-4375-a2c2-f30404382259", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), null, null, false, false, "Csproj Location", "CsprojLocation", 3, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderAdditionalConfigs",
                columns: new[] { "Id", "AllowedValues", "ConcurrencyStamp", "Created", "Hint", "IsInputMasked", "IsRequired", "IsSecret", "Label", "Name", "TaskProviderId", "Type", "Updated" },
                values: new object[] { 1, null, "c48cafcc-b3e9-4375-a2c2-f30404382258", new DateTime(2018, 9, 28, 7, 23, 37, 58, DateTimeKind.Utc), "Please enter the email address that you wish to be used as an administrator of the project", null, true, false, "Admin Email", "AdminEmail", 1, "string", null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 3, "21222bae-5e15-432c-ae4f-e671cb116d09", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 25, "21222bae-5e15-432c-ae4f-e671cb116d20", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 6, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 26, "21222bae-5e15-432c-ae4f-e671cb116d21", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 27, "21222bae-5e15-432c-ae4f-e671cb116d22", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 12, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 28, "21222bae-5e15-432c-ae4f-e671cb116d23", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 20, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 29, "21222bae-5e15-432c-ae4f-e671cb116d24", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 21, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 30, "21222bae-5e15-432c-ae4f-e671cb116d25", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 22, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 31, "21222bae-5e15-432c-ae4f-e671cb116d26", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 23, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 32, "21222bae-5e15-432c-ae4f-e671cb116d27", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 24, 6, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 2, "21222bae-5e15-432c-ae4f-e671cb116d08", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 1, "21222bae-5e15-432c-ae4f-e671cb116d07", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 1, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 33, "21222bae-5e15-432c-ae4f-e671cb116d28", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 25, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 34, "21222bae-5e15-432c-ae4f-e671cb116d29", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 26, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 35, "21222bae-5e15-432c-ae4f-e671cb116d2a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 27, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 36, "21222bae-5e15-432c-ae4f-e671cb116d2b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 28, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 37, "21222bae-5e15-432c-ae4f-e671cb116d2c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 29, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 38, "21222bae-5e15-432c-ae4f-e671cb116d2d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 30, 7, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 4, "21222bae-5e15-432c-ae4f-e671cb116d0a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 9, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 13, "21222bae-5e15-432c-ae4f-e671cb116d13", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 17, 2, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 5, "21222bae-5e15-432c-ae4f-e671cb116d0b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 10, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 7, "21222bae-5e15-432c-ae4f-e671cb116d0d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 12, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 16, "21222bae-5e15-432c-ae4f-e671cb116d16", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 3, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 17, "21222bae-5e15-432c-ae4f-e671cb116d17", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 4, 4, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 18, "21222bae-5e15-432c-ae4f-e671cb116d18", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 4, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 19, "21222bae-5e15-432c-ae4f-e671cb116d19", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 18, 4, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 20, "21222bae-5e15-432c-ae4f-e671cb116d1a", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 19, 4, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 12, "21222bae-5e15-432c-ae4f-e671cb116d12", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 16, 2, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 6, "21222bae-5e15-432c-ae4f-e671cb116d0c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 11, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 11, "21222bae-5e15-432c-ae4f-e671cb116d11", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 15, 2, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 21, "21222bae-5e15-432c-ae4f-e671cb116d1b", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 5, 5, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 22, "21222bae-5e15-432c-ae4f-e671cb116d1c", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 5, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 23, "21222bae-5e15-432c-ae4f-e671cb116d1d", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 8, 5, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 24, "21222bae-5e15-432c-ae4f-e671cb116d1f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 14, 5, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 10, "21222bae-5e15-432c-ae4f-e671cb116d10", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 2, 2, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 9, "21222bae-5e15-432c-ae4f-e671cb116d0f", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 14, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 8, "21222bae-5e15-432c-ae4f-e671cb116d0e", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 13, 1, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 15, "21222bae-5e15-432c-ae4f-e671cb116d15", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 7, 3, null });

            migrationBuilder.InsertData(
                table: "TaskProviderTags",
                columns: new[] { "Id", "ConcurrencyStamp", "Created", "TagId", "TaskProviderId", "Updated" },
                values: new object[] { 14, "21222bae-5e15-432c-ae4f-e671cb116d14", new DateTime(2019, 4, 10, 15, 50, 43, 176, DateTimeKind.Utc), 3, 3, null });

            migrationBuilder.InsertData(
                table: "UserProfile",
                columns: new[] { "Id", "ApplicationUserId", "AvatarFileId", "ConcurrencyStamp", "Created", "ExternalAccountIds", "FirstName", "IsActive", "LastName", "Updated" },
                values: new object[] { 1, 1, null, "99aa6fde-2675-4aa9-a60d-e45ba72fb9d0", new DateTime(2018, 8, 23, 10, 4, 6, 797, DateTimeKind.Utc), null, null, true, null, null });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_CatapultEngineProfile_CatapultEngineId",
                table: "CatapultEngineProfile",
                column: "CatapultEngineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExternalServiceProperties_ExternalServiceTypeId",
                table: "ExternalServiceProperties",
                column: "ExternalServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalServices_ExternalServiceTypeId",
                table: "ExternalServices",
                column: "ExternalServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDefinitions_ProjectId",
                table: "JobDefinitions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_JobQueues_ProjectId",
                table: "JobQueues",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTaskDefinitions_JobDefinitionId",
                table: "JobTaskDefinitions",
                column: "JobDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDataModelProperties_ProjectDataModelId",
                table: "ProjectDataModelProperties",
                column: "ProjectDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDataModelProperties_RelatedProjectDataModelId",
                table: "ProjectDataModelProperties",
                column: "RelatedProjectDataModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDataModels_ProjectId",
                table: "ProjectDataModels",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectMemberRoleId",
                table: "ProjectMembers",
                column: "ProjectMemberRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderAdditionalConfigs_TaskProviderId",
                table: "TaskProviderAdditionalConfigs",
                column: "TaskProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderTags_TagId",
                table: "TaskProviderTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskProviderTags_TaskProviderId",
                table: "TaskProviderTags",
                column: "TaskProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ApplicationUserId",
                table: "UserProfile",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_AvatarFileId",
                table: "UserProfile",
                column: "AvatarFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatapultEngineProfile");

            migrationBuilder.DropTable(
                name: "ExternalAccountType");

            migrationBuilder.DropTable(
                name: "ExternalServiceProperties");

            migrationBuilder.DropTable(
                name: "ExternalServices");

            migrationBuilder.DropTable(
                name: "HelpContexts");

            migrationBuilder.DropTable(
                name: "JobCounters");

            migrationBuilder.DropTable(
                name: "JobQueues");

            migrationBuilder.DropTable(
                name: "JobTaskDefinitions");

            migrationBuilder.DropTable(
                name: "ProjectDataModelProperties");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "TaskProviderAdditionalConfigs");

            migrationBuilder.DropTable(
                name: "TaskProviderTags");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "ExternalServiceTypes");

            migrationBuilder.DropTable(
                name: "JobDefinitions");

            migrationBuilder.DropTable(
                name: "ProjectDataModels");

            migrationBuilder.DropTable(
                name: "ProjectMemberRoles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TaskProviders");

            migrationBuilder.DropTable(
                name: "ManagedFiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
